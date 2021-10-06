using System.Linq;
using System.Net;
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
                    .WithMessage($"User {Messages.NOT_NULL}")
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
                var user = await context.Users.Where(x => x.Username == request.login.Username).SingleOrDefaultAsync(cancellationToken);
                if (user == null)
                {
                    var message = $"{Messages.INVALID_CREDENTIALS}";
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = message});
                }

                if (!user.Hash.SequenceEqual(await hasher.HashAsync(request.login.Password ?? throw new InvalidOperationException(), user.Salt)))
                {
                    var message = $"{Messages.INVALID_CREDENTIALS}";
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = message});
                }
                
                // Maps to a model that includes a JWT token for Angular
                var loggedInUser = mapper.Map<User, LoggedIn>(user);
                loggedInUser.Token = bigGenerator.CreateToken(user.Username ?? throw new InvalidOperationException());

                return loggedInUser;
            }
        }
    }
}