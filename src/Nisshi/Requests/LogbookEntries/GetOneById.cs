using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure.Errors;
using System.Net;

namespace Nisshi.Requests.LogbookEntries
{
  public class GetOneById
    {
        public record Query(int id) : IRequest<LogbookEntry>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, LogbookEntry>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<LogbookEntry> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();
                if (string.IsNullOrEmpty(username))
                    throw new RestException(HttpStatusCode.Unauthorized, Message.NotLoggedIn);

                var data = await context.LogbookEntries
                    .Include(x => x.Owner)
                    .Include(x => x.Aircraft)
                        .ThenInclude(x => x.Model)
                    .FirstOrDefaultAsync(x => x.Id == request.id 
                        && x.Owner.Username.ToUpper() == username.ToUpper(), cancellationToken);

                return data;
            }
        }
    }
}