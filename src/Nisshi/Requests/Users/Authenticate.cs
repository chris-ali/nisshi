using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using System;
using Nisshi.Infrastructure.Security;
using AutoMapper;
using Nisshi.Models.Users;
using System.Security.Authentication;

namespace Nisshi.Requests.Users
{
    public class Login
    {
        public record Command(Authenticate login) : IRequest<LoggedIn>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.login).NotNull()
                    .WithMessage(Message.NotNull.ToString())
                    .SetValidator(new Authenticate.AuthenticateValidator());
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
                var user = await context.Users
                    .SingleOrDefaultAsync(x => x.Username == request.login.Username, cancellationToken);

                if (user == null)
                    throw new AuthenticationException(Message.InvalidCredentials.ToString());

                var authenticated = user.Hash.SequenceEqual(await hasher.HashAsync(request.login.Password ?? throw new InvalidOperationException(),
                    user.Salt, cancellationToken));

                if (!authenticated)
                    throw new AuthenticationException(Message.InvalidCredentials.ToString());

                // Maps to a model that includes a JWT token for Angular
                var loggedInUser = mapper.Map<User, LoggedIn>(user);
                loggedInUser.Token = bigGenerator
                    .CreateToken(user.Username ?? throw new InvalidOperationException(), user.UserType.ToString());

                return loggedInUser;
            }
        }
    }
}