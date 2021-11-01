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

namespace Nisshi.Requests.LogbookEntries
{
    public class Create
    {
        public record Command(LogbookEntry logbookEntry) : IRequest<LogbookEntry>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.logbookEntry).NotNull()
                    .WithMessage(Message.NotNull.ToString())
                    .SetValidator(new LogbookEntry.LogbookEntryValidator());
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, LogbookEntry>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<LogbookEntry> Handle(Command request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var currentUser = await context.Users
                    .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

                request.logbookEntry.DateCreated = request.logbookEntry.DateUpdated = DateTime.Now;
                request.logbookEntry.Owner = currentUser;

                await context.AddAsync<LogbookEntry>(request.logbookEntry, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.logbookEntry;
            }
        }
    }
}
