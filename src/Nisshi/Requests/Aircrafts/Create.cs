using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts
{
    public class Create
    {
        public record Command(Aircraft aircraft) : IRequest<Aircraft>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.aircraft).NotNull().WithMessage($"Aircraft {Messages.NOT_NULL}");
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, Aircraft>
        {
            private readonly ICurrentUserAccessor accessor;


            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Aircraft> Handle(Command request, CancellationToken cancellationToken)
            { 
                var username = accessor.GetCurrentUserName();
                
                if (string.IsNullOrEmpty(username))
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = Messages.NOT_LOGGED_IN });
                
                var currentUser = await context.Users
                    .Where(x => x.Username == username)
                    .FirstOrDefaultAsync(cancellationToken);

                request.aircraft.DateCreated = request.aircraft.DateUpdated = DateTime.Now;
                request.aircraft.Owner = currentUser;

                await context.AddAsync<Aircraft>(request.aircraft, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.aircraft;
            }
        }
    }
}