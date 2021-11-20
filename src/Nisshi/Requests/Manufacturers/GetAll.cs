using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Models;

namespace Nisshi.Requests.Manufacturers
{
    public class GetAll
    {
        public record Query() : IRequest<IList<Manufacturer>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<Manufacturer>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<Manufacturer>> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.Manufacturers.ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
