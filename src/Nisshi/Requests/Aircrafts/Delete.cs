using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts
{
    public class Delete
    {
        public record Command(int id) : IRequest<Aircraft>;

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
                    throw new RestException(HttpStatusCode.Unauthorized, Message.NotLoggedIn);

                var data = await context.Aircraft
                    .Include(x => x.Owner)
                    .FirstOrDefaultAsync(x => x.Id == request.id 
                        && x.Owner.Username.ToUpper() == username.ToUpper(), cancellationToken);

                if (data == null)
                    throw new RestException(HttpStatusCode.NotFound, Message.ItemDoesNotExist);

                // Should also cascade to remove logbook entries
                context.Remove(data);
                await context.SaveChangesAsync();

                return data;
            }
        }
    }
}