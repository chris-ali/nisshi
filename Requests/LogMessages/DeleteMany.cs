using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Nisshi.Requests.LogMessages
{
  public class DeleteMany
    {
        public record Command()  : IRequest<IEnumerable<LogMessage>>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, IEnumerable<LogMessage>>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(HeroesDbContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<IEnumerable<LogMessage>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Set current user as ownner
                var userName = accessor.GetCurrentUserName();

                if (string.IsNullOrEmpty(userName))
                {
                    var message = $"No logged in user found!";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = message });
                }

                var data = context.Messages.Include(x => x.Owner).Where(x => x.Owner.UserName == userName);

                if (data == null) 
                {
                    var message = $"No messages found to delete for user: {userName}";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message });
                }
                
                // logger.LogDebug($"Found {data.Count()} messages from {request.userName} to delete...");

                context.RemoveRange(data);
                await context.SaveChangesAsync(cancellationToken);
                
                // logger.LogDebug($"...deleted successfully!");

                return data;    
            }
        }
    }
}