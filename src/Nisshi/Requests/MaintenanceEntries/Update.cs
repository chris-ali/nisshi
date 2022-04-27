using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;

namespace Nisshi.Requests.MaintenanceEntries
{
    public class Update
    {
        public record Command(MaintenanceEntry maintenanceEntry) : IRequest<MaintenanceEntry>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.maintenanceEntry).NotNull()
                    .WithMessage(Message.NotNull.ToString())
                    .SetValidator(new MaintenanceEntry.MaintenanceEntryValidator());
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, MaintenanceEntry>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<MaintenanceEntry> Handle(Command request, CancellationToken cancellationToken)
            {
                var data = await context.FindAsync<MaintenanceEntry>(new object[] { request.maintenanceEntry.Id }, cancellationToken);

                if (data == null)
                    throw new DomainException(typeof(MaintenanceEntry), Message.ItemDoesNotExist);

                var username = accessor.GetCurrentUserName();

                var user = await context.Users
                    .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

                Update(ref data, request.maintenanceEntry);
                data.DateUpdated = DateTime.Now;
                data.Owner = user;

                context.Update<MaintenanceEntry>(data);
                await context.SaveChangesAsync(cancellationToken);

                return data;
            }

            /// <summary>
            /// Updates MaintenanceEntry object from database with object in request
            /// </summary>
            /// <param name="toBeUpdated"></param>
            /// <param name="toUpdateWith"></param>
            private void Update(ref MaintenanceEntry toBeUpdated, MaintenanceEntry toUpdateWith)
            {
                toBeUpdated.Comments = toUpdateWith.Comments;
                toBeUpdated.Duration = toUpdateWith.Duration;
                toBeUpdated.MilesPerformed = toUpdateWith.MilesPerformed;
                toBeUpdated.PerformedBy = toUpdateWith.PerformedBy;
                toBeUpdated.RepairPrice = toUpdateWith.RepairPrice;
                toBeUpdated.Type = toUpdateWith.Type;
                toBeUpdated.WorkDescription = toUpdateWith.WorkDescription;
            }
        }
    }
}
