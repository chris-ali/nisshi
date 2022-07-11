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
namespace Nisshi.Requests.Vehicles
{
    public class Create
    {
        public record Command(Vehicle vehicle) : IRequest<Vehicle>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.vehicle).NotNull()
                    .WithMessage(Message.NotNull.ToString())
                    .SetValidator(new Vehicle.VehicleValidator());
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, Vehicle>
        {
            private readonly ICurrentUserAccessor accessor;


            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Vehicle> Handle(Command request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var currentUser = await context.Users
                    .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

                request.vehicle.DateCreated = request.vehicle.DateUpdated = DateTime.Now;
                request.vehicle.Owner = currentUser;

                await context.AddAsync<Vehicle>(request.vehicle, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.vehicle;
            }
        }
    }
}
