using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Infrastructure.Security;
using Nisshi.Models.Users;

namespace Nisshi.Requests.Users
{
    public class ChangePassword
    {
        public record Command(ChangePasswordModel change) : IRequest<User>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.change).NotNull()
                    .WithMessage(Message.NotNull.ToString())
                    .SetValidator(new ChangePasswordModel.ChangePasswordValidator());
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, User>
        {
            private readonly ICurrentUserAccessor accessor;
            private readonly IPasswordHasher hasher;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor, IPasswordHasher hasher) : base(context)
            {
                this.accessor = accessor;
                this.hasher = hasher;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var user = await context.Users.SingleOrDefaultAsync(x => x.Username == username, cancellationToken);

                if (user == null)
                    throw new DomainException(typeof(User), Message.ItemDoesNotExist);

                var authenticated = user.Hash.SequenceEqual(await hasher.HashAsync(request.change.OldPassword ??
                    throw new InvalidOperationException(), user.Salt, cancellationToken));

                if (!authenticated)
                    throw new AuthenticationException(Message.InvalidCredentials.ToString());

                user.DateUpdated = user.LastPasswordChangedDate = DateTime.Now;

                var iodized = Guid.NewGuid().ToByteArray();
                user.Salt = iodized;
                user.Hash = await hasher.HashAsync(request.change.NewPassword, iodized, cancellationToken);

                context.Update<User>(user);
                await context.SaveChangesAsync(cancellationToken);

                return user;
            }
        }
    }
}
