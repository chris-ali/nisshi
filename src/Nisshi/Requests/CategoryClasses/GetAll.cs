using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Models;

namespace Nisshi.Requests.CategoryClasses
{
    public class GetAll
    {
        public record Query() : IRequest<IList<CategoryClass>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<CategoryClass>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<CategoryClass>> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.CategoryClasses.ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
