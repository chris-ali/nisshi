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

namespace Nisshi.Requests.LogbookEntries
{
    public class Create
    {
        public record Command(LogbookEntry logbookEntry) : IRequest<LogbookEntry>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.logbookEntry).NotNull().WithMessage($"Logbook Entry {Messages.NOT_NULL}");
            }
        }

        public class CommandHandler : BaseRequest, IRequestHandler<Command, LogbookEntry>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<LogbookEntry> Handle(Command request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();
                
                if (string.IsNullOrEmpty(username))
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = Messages.NOT_LOGGED_IN });
                
                var currentUser = await context.Users
                    .Where(x => x.Username == username)
                    .FirstOrDefaultAsync(cancellationToken);

                request.logbookEntry.DateCreated = request.logbookEntry.DateUpdated = DateTime.Now;
                request.logbookEntry.Owner = currentUser;

                await context.AddAsync<LogbookEntry>(request.logbookEntry, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.logbookEntry;
            }
        }
    }
}