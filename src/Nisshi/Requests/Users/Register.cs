using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using System;
using Nisshi.Infrastructure.Security;
using AutoMapper;

namespace Nisshi.Requests.Users
{
    public class Register
    {
        public record Command(UserRegistration registration) : IRequest<UserLoggedIn>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.registration).NotNull()
                    .WithMessage($"User {Messages.NOT_NULL}")
                    .SetValidator(new UserRegistration.RegistrationValidator());
            }
        }

        public class CommandHandler : BaseRequest, IRequestHandler<Command, UserLoggedIn>
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

            public async Task<UserLoggedIn> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await context.Users.Where(x => x.Username == request.registration.Username).AnyAsync(cancellationToken))
                {
                    var message = $"Username {Messages.ALREADY_EXISTS}";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message});
                }

                if (await context.Users.Where(x => x.Email == request.registration.Email).AnyAsync(cancellationToken))
                {
                    var message = $"Email {Messages.ALREADY_EXISTS}";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message});
                }

                var iodized = Guid.NewGuid().ToByteArray();
                var user = new User 
                {
                    Username = request.registration.Username,
                    Email = request.registration.Email,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Salt = iodized,
                    Hash = await hasher.Hash(request.registration.Password 
                            ?? throw new InvalidOperationException(), iodized)
                };
                
                await context.AddAsync<User>(user);
                await context.SaveChangesAsync(cancellationToken);

                // Maps to a model that includes a JWT token for Angular
                var loggedInUser = mapper.Map<User, UserLoggedIn>(user);

                return loggedInUser;
            }
        }
    }
}