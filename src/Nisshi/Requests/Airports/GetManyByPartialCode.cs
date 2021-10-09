using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Nisshi.Requests.Airports 
{
    public class GetManyByPartialCode
    {
        public record Query(string partialCode) : IRequest<IList<Airport>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<Airport>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<Airport>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.partialCode) || request.partialCode.Length < 3)
                    return null;
                
                var data = await context.Airports
                    .Where(x => x.AirportCode.ToUpper().StartsWith(request.partialCode.ToUpper()))
                    .ToListAsync(cancellationToken);
                
                return data;
            }
        }
    }
}