using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Models;

namespace Nisshi.Requests.Analytics
{
    public class GetSumTotals
    {
        public record Query() : IRequest<SumTotals>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, SumTotals>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<SumTotals> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.LogbookEntries
                    .Include(x => x.Owner)
                    .Include(x => x.Aircraft)
                        .ThenInclude(x => x.Model)
                        .ThenInclude(x => x.CategoryClass)
                    .Where(x => x.Owner.Username.ToLower(CultureInfo.InvariantCulture) ==
                        username.ToLower(CultureInfo.InvariantCulture))
                    .ToListAsync(cancellationToken);

                return new SumTotals {
                    TotalTimeSum = data.Sum(t => t.TotalFlightTime),
                    MultiSum = data.Sum(t => t.MultiEngine),
                    InstrumentSum = data.Sum(t => t.IMC),
                    NightSum = data.Sum(t => t.Night),
                    CrossCountrySum = data.Sum(t => t.CrossCountry),
                    TurbineSum = data.Sum(t => t.Turbine),
                    PICSum = data.Sum(t => t.PIC),
                    SICSum = data.Sum(t => t.SIC),
                    DualGivenSum = data.Sum(t => t.DualGiven),
                };
            }
        }
    }
}
