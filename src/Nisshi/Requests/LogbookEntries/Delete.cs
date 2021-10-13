using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Nisshi.Requests.LogbookEntries
{
    public class Delete
    {
        public record Command(int id) : IRequest<LogbookEntry>;

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
                if (string.IsNullOrEmpty(username))
                    throw new RestException(HttpStatusCode.Unauthorized, Message.NotLoggedIn);

                var data = await context.LogbookEntries
                    .Include(x => x.Owner)
                    .FirstOrDefaultAsync(x => x.Id == request.id
                        && x.Owner.Username.ToUpper() == username.ToUpper(), cancellationToken);
                
                if (data == null)
                    throw new RestException(HttpStatusCode.NotFound, Message.ItemDoesNotExist);

                context.Remove(data);
                await context.SaveChangesAsync();

                return data;
            }
        }
    }
}