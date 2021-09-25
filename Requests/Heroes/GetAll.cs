using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Nisshi.Requests.Heroes 
{
    public class GetAll
    {
        public record Query() : IRequest<IEnumerable<Hero>>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, IEnumerable<Hero>>
        {
            public QueryHandler(HeroesDbContext context) : base(context)
            {
            }

            public async Task<IEnumerable<Hero>> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.Heroes.ToListAsync(cancellationToken);
                // logger.LogDebug($"Found {data.Count} heroes to return...");

                return data;
            }
        }
    }
}