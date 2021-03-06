using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;

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
                RuleFor(x => x.aircraft).NotNull()
                    .WithMessage(Message.NotNull.ToString())
                    .SetValidator(new Aircraft.AircraftValidator());
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

                var currentUser = await context.Users
                    .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

                request.aircraft.DateCreated = request.aircraft.DateUpdated = DateTime.Now;
                request.aircraft.Owner = currentUser;

                await context.AddAsync<Aircraft>(request.aircraft, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.aircraft;
            }
        }
    }
}
