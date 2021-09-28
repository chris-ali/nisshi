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
    public class GetMany
    {
        public record Query() : IRequest<IEnumerable<LogMessage>>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, IEnumerable<LogMessage>>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<IEnumerable<LogMessage>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Set current user as ownner
                var userName = accessor.GetCurrentUserName();

                if (string.IsNullOrEmpty(userName))
                {
                    var message = $"No logged in user found!";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = message });
                }
                
                var data = await context.Messages
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Username == userName)
                    .ToListAsync(cancellationToken);

                // logger.LogDebug($"Found {data.Count} messages to return for user {request.userName}...");

                return data;
            }
        }
    }
}