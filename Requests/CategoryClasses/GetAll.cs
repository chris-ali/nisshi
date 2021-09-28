using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Nisshi.Requests.CategoryClasses 
{
    public class GetAll
    {
        public record Query() : IRequest<IList<CategoryClass>>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, IList<CategoryClass>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<CategoryClass>> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.CategoryClass.ToListAsync(cancellationToken);
                
                return data;
            }
        }
    }
}