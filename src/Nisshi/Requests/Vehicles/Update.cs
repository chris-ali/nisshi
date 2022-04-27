using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;

namespace Nisshi.Requests.Vehicles
{
    public class Update
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
                var data = await context.FindAsync<Vehicle>(new object[] { request.vehicle.Id }, cancellationToken);

                if (data == null)
                    throw new DomainException(typeof(Vehicle), Message.ItemDoesNotExist);

                var username = accessor.GetCurrentUserName();

                var user = await context.Users
                    .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

                Update(ref data, request.vehicle);
                data.DateUpdated = DateTime.Now;
                data.Owner = user;

                context.Update<Vehicle>(data);
                await context.SaveChangesAsync(cancellationToken);

                return data;
            }

            /// <summary>
            /// Updates Vehicle object from database with object in request
            /// </summary>
            /// <param name="toBeUpdated"></param>
            /// <param name="toUpdateWith"></param>
            private void Update(ref Vehicle toBeUpdated, Vehicle toUpdateWith)
            {
                toBeUpdated.Vin = toUpdateWith.Vin;
                toBeUpdated.InspectionDue = toUpdateWith.InspectionDue;
                toBeUpdated.LastInspection = toUpdateWith.LastInspection;
                toBeUpdated.RegistrationDue = toUpdateWith.RegistrationDue;
                toBeUpdated.LastRegistration = toUpdateWith.LastRegistration;
                toBeUpdated.Make = toUpdateWith.Make;
                toBeUpdated.Model = toUpdateWith.Model;
                toBeUpdated.Trim = toUpdateWith.Trim;
                toBeUpdated.Year = toUpdateWith.Year;
                toBeUpdated.Notes = toUpdateWith.Notes;
            }
        }
    }
}
