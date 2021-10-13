using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using MediatR;
using System;
using FluentValidation;
using Nisshi.Models.Users;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure.Security;

namespace Nisshi.Requests.Users
{
  public class UpdateProfile
    {
        public record Command(Profile edit) : IRequest<User>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.edit).NotNull().WithMessage(Message.NotNull.ToString());
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
                if (string.IsNullOrEmpty(username))
                    throw new RestException(HttpStatusCode.Unauthorized, Message.NotLoggedIn);

                var user = await context.Users.Where(x => x.Username == username).SingleOrDefaultAsync(cancellationToken);
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, Message.ItemDoesNotExist);
                                
                Update(ref user, request.edit);
                user.DateUpdated = DateTime.Now;

                if (!string.IsNullOrEmpty(request.edit.Password))
                {
                    var iodized = Guid.NewGuid().ToByteArray();
                    user.Salt = iodized;
                    user.Hash = await hasher.HashAsync(request.edit.Password, iodized, cancellationToken);
                }

                context.Update<User>(user);
                await context.SaveChangesAsync(cancellationToken);

                return user;
            }

            /// <summary>
            /// Updates User object from database with object in request
            /// </summary>
            /// <param name="toBeUpdated"></param>
            /// <param name="toUpdateWith"></param>
            private void Update(ref User toBeUpdated, Profile toUpdateWith) 
            {
                toBeUpdated.CertificateNumber = toUpdateWith.CertificateNumber;
                toBeUpdated.CFIExpiration = toUpdateWith.CFIExpiration;
                toBeUpdated.FirstName = toUpdateWith.FirstName;
                toBeUpdated.IsInstructor = toUpdateWith.IsInstructor;
                toBeUpdated.LastBFR = toUpdateWith.LastBFR;
                toBeUpdated.LastMedical = toUpdateWith.LastMedical;
                toBeUpdated.LastName = toUpdateWith.LastName;
                toBeUpdated.License = toUpdateWith.License;
                toBeUpdated.MonthsToMedical = toUpdateWith.MonthsToMedical;
                toBeUpdated.Preferences = toUpdateWith.Preferences;
            }
        }
    }
}