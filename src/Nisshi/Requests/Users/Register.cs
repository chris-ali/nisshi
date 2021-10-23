using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Enums;
using Nisshi.Infrastructure.Errors;
using Nisshi.Infrastructure.Security;
using Nisshi.Models.Users;

namespace Nisshi.Requests.Users
{
    public class Register
    {
        public record Command(Registration registration) : IRequest<LoggedIn>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.registration).NotNull()
                    .WithMessage(Message.NotNull.ToString())
                    .SetValidator(new Registration.RegistrationValidator());
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, LoggedIn>
        {
            private readonly IPasswordHasher hasher;
            private readonly IJwtTokenGenerator bigGenerator;
            private readonly IMapper mapper;

            public CommandHandler(NisshiContext context,
                                IPasswordHasher hasher,
                                IJwtTokenGenerator bigGenerator,
                                IMapper mapper) : base(context)
            {
                this.hasher = hasher;
                this.bigGenerator = bigGenerator;
                this.mapper = mapper;
            }

            public async Task<LoggedIn> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await context.Users.Where(x => x.Username == request.registration.Username).AnyAsync(cancellationToken))
                    throw new InvalidCredentialException(Message.UsernameExists.ToString());

                if (await context.Users.Where(x => x.Email == request.registration.Email).AnyAsync(cancellationToken))
                    throw new InvalidCredentialException(Message.EmailExists.ToString());

                var iodized = Guid.NewGuid().ToByteArray();
                var user = new User
                {
                    Username = request.registration.Username,
                    Email = request.registration.Email,
                    UserType = UserType.User,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Salt = iodized,
                    Hash = await hasher.HashAsync(request.registration.Password
                            ?? throw new InvalidOperationException(), iodized, cancellationToken)
                };

                await context.AddAsync<User>(user);
                await context.SaveChangesAsync(cancellationToken);

                // Maps to a model that includes a JWT token for Angular
                var loggedInUser = mapper.Map<User, LoggedIn>(user);
                loggedInUser.Token = bigGenerator
                    .CreateToken(user.Username ?? throw new InvalidOperationException(), user.UserType.ToString());

                return loggedInUser;
            }
        }
    }
}
