using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Nisshi.Requests.Heroes
{
  public class GetOneById
    {
        public record Query(int id) : IRequest<Hero>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, Hero>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<Hero> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.FindAsync<Hero>(new object[] { request.id }, cancellationToken);
            
                // if (data != null)
                //     logger.LogDebug($"Found hero: {data.Id} - {data.Name} to return...");
                // else 
                //     logger.LogWarning($"No hero found for id: {request.id}");

                return data;
            }
        }
    }
}