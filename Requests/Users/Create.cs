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

namespace Nisshi.Requests.Users
{
    public class Create
    {
        // TODO make user wrapper here that has registration details; use as input to endpoint; Models.User should just have JWT token

        public record Command(User user) : IRequest<User>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.user).NotNull().WithMessage($"User {Messages.NOT_NULL}");
            }
        }

        public class CommandHandler : BaseRequest, IRequestHandler<Command, User>
        {
            public CommandHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await context.Users.Where(x => x.Username == request.user.Username).AnyAsync(cancellationToken))
                {
                    var message = $"Username {Messages.ALREADY_EXISTS}";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message});
                }

                if (await context.Users.Where(x => x.Email == request.user.Email).AnyAsync(cancellationToken))
                {
                    var message = $"Email {Messages.ALREADY_EXISTS}";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message});
                }

                request.user.DateCreated = request.user.DateUpdated = DateTime.Now;

                // TODO Create token hash and salt here

                await context.AddAsync<User>(request.user);
                await context.SaveChangesAsync(cancellationToken);

                return request.user;
            }
        }
    }
}