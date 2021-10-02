using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Nisshi.Requests.LogbookEntries
{
  public class GetOneById
    {
        public record Query(int id) : IRequest<LogbookEntry>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, LogbookEntry>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<LogbookEntry> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.LogbookEntries
                    .Include(x => x.Aircraft)
                        .ThenInclude(x => x.Model)
                    .FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);

                return data;
            }
        }
    }
}