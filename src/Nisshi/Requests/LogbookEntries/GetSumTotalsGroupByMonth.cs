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
    public class GetSumTotalsGroupByMonth
    {
        public record Query() : IRequest<IList<TotalsByMonth>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<TotalsByMonth>>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<IList<TotalsByMonth>> Handle(Query request, CancellationToken cancellationToken)
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
                    .Select(s => new TotalsByMonth {
                        Month = s.Key.Month,
                        Year = s.Key.Year,
                        TotalTimeSum = s.Sum(t => t.TotalFlightTime),
                        InstrumentSum = s.Sum(t => t.IMC),
                        NightSum = s.Sum(t => t.Night),
                        CrossCountrySum = s.Sum(t => t.CrossCountry),
                        TurbineSum = s.Sum(t => t.Turbine),
                        PICSum = s.Sum(t => t.PIC),
                        SICSum = s.Sum(t => t.SIC),
                        DualGivenSum = s.Sum(t => t.DualGiven),
                    })
                    .OrderByDescending(x => x.Year)
                    .ThenByDescending(x => x.Month)
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}