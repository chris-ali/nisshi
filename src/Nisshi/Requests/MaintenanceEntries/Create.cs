using System;
using System.Linq;
using System.Net;
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
    public class Create
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
                var username = accessor.GetCurrentUserName();

                var currentUser = await context.Users
                    .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

                request.maintenanceEntry.DateCreated = request.maintenanceEntry.DateUpdated = DateTime.Now;
                request.maintenanceEntry.Owner = currentUser;

                await context.AddAsync<MaintenanceEntry>(request.maintenanceEntry, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.maintenanceEntry;
            }
        }
    }
}
