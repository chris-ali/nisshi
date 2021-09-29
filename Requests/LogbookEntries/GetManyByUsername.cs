using System.Collections.Generic;
using System.Linq;
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
    public class GetManyByUsername
    {
        public record Query(string username) : IRequest<IList<LogbookEntry>>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, IList<LogbookEntry>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<LogbookEntry>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.username))
                {
                    var message = $"Must provide username";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message });
                }
                
                var data = await context.LogbookEntries
                    .Include(x => x.Aircraft)
                        .ThenInclude(x => x.Model)
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Username == request.username)
                    .ToListAsync(cancellationToken);
                
                if (data.Count == 0)
                {
                    var message = $"No logbook entries found for user: {request.username}";
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message });
                }
                
                return data;
            }
        }
    }
}