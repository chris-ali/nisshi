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
    public class GetManyByPartialName
    {
        public record Query(string partialName) : IRequest<IList<Model>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<Model>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.partialName) || request.partialName.Length < 2)
                    return null;
                
                var data = await context.Models
                    .Where(x => x.ModelName.ToUpper().StartsWith(request.partialName.ToUpper()))
                    .ToListAsync(cancellationToken);
                
                return data;
            }
        }
    }
}