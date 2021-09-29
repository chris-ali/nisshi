using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts
{
  public class GetOneById
    {
        public record Query(int id) : IRequest<Aircraft>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, Aircraft>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<Aircraft> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.Aircraft
                    .Include(x => x.Model)
                    .FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);

                return data;
            }
        }
    }
}