using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Models;

namespace Nisshi.Requests.Models
{
    public class GetOneById
    {
        public record Query(int id) : IRequest<Model>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, Model>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.Models
                    .Include(x => x.CategoryClass)
                    .FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);

                return data;
            }
        }
    }
}
