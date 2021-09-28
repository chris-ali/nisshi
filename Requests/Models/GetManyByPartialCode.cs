using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Nisshi.Requests.Models 
{
    public class GetManyByPartialCode
    {
        public record Query(string partialCode) : IRequest<IList<Model>>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, IList<Model>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.partialCode) || request.partialCode.Length < 3)
                    return null;
                
                var data = await context.Models
                    .Where(x => x.ModelName.StartsWith(request.partialCode))
                    .ToListAsync(cancellationToken);
                
                return data;
            }
        }
    }
}