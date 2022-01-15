using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Models;

namespace Nisshi.Requests.LogbookEntries
{
    public class GetSumTotalTimeGroupByMonth
    {
        public record Query() : IRequest<IList<TotalTimeByMonth>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<TotalTimeByMonth>>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<IList<TotalTimeByMonth>> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.LogbookEntries
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Username.ToLower(CultureInfo.InvariantCulture) ==
                        username.ToLower(CultureInfo.InvariantCulture))
                    .GroupBy(g => new {
                        Month = g.FlightDate.Value.Month,
                        Year = g.FlightDate.Value.Year
                    })
                    .Select(s => new TotalTimeByMonth {
                        Month = s.Key.Month,
                        Year = s.Key.Year,
                        TotalTimeSum = s.Sum(t => t.TotalFlightTime)
                    }).ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
