using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Nisshi.Requests.Manufacturers 
{
    public class GetManyByPartialName
    {
        public record Query(string partialName) : IRequest<IList<Manufacturer>>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, IList<Manufacturer>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<Manufacturer>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.partialName) || request.partialName.Length < 3)
                    return null;
                
                var data = await context.Manufacturers
                    .Where(x => x.ManufacturerName.ToUpper().StartsWith(request.partialName.ToUpper()))
                    .ToListAsync(cancellationToken);
                
                return data;
            }
        }
    }
}