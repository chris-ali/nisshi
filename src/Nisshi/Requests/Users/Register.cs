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
                    throw new RestException(HttpStatusCode.BadRequest, Message.UsernameExists);
                
                if (await context.Users.Where(x => x.Email == request.registration.Email).AnyAsync(cancellationToken))
                    throw new RestException(HttpStatusCode.BadRequest, Message.EmailExists);
                
                var iodized = Guid.NewGuid().ToByteArray();
                var user = new User 
                {
                    Username = request.registration.Username,
                    Email = request.registration.Email,
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
                loggedInUser.Token = bigGenerator.CreateToken(user.Username ?? throw new InvalidOperationException());

                return loggedInUser;
            }
        }
    }
}