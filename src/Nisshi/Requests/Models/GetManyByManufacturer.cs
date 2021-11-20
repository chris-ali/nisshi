using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Models;

namespace Nisshi.Requests.Models
{
    public class GetManyByManufacturer
    {
        public record Query(int IdManufacturer) : IRequest<IList<Model>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<Model>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.Models
                    .Where(x => x.IdManufacturer == request.IdManufacturer)
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
