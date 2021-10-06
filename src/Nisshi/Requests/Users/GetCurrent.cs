using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Models.Users;

namespace Nisshi.Requests.Users 
{
    public class GetCurrent
    {
        public record Query() : IRequest<User>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, User>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var userName = accessor.GetCurrentUserName();
                var data = await context.Users.FirstOrDefaultAsync(x => x.Username == userName, cancellationToken);

                if (string.IsNullOrEmpty(userName) || data == null) 
                {
                    var message = $"No current user found in {(data == null ? "database" : "HTTP context")} ";
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message});
                }

                return data;
            }
        }
    }
}