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

namespace Nisshi.Requests.LogbookEntries
{
    public class Create
    {
        public record Command(LogbookEntry logbookEntry) : IRequest<LogbookEntry>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, LogbookEntry>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<LogbookEntry> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.logbookEntry == null) 
                {
                    var message = $"No logbook entry data found in request";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message });
                }

                var username = accessor.GetCurrentUserName();

                if (string.IsNullOrEmpty(username))
                {
                    var message = $"No logged in user found!";
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = message });
                }

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