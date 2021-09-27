using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Nisshi.Requests.Users 
{
    public class GetCurrent
    {
        public record Query() : IRequest<User>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, User>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var userName = accessor.GetCurrentUserName();
                var data = await context.Users.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

                if (string.IsNullOrEmpty(userName) || data == null) 
                {
                    var message = $"No current user found in {(data == null ? "database" : "HTTP context")} ";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message});
                }

                // logger.LogDebug($"Found user: {data.UserName}...");

                return data;
            }
        }
    }
}