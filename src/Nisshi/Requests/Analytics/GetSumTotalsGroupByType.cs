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
    public class GetSumTotalsGroupByType
    {
        public record Query() : IRequest<IList<TotalsByType>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<TotalsByType>>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<IList<TotalsByType>> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.LogbookEntries
                    .Include(x => x.Owner)
                    .Include(x => x.Aircraft)
                        .ThenInclude(x => x.Model)
                    .Where(x => x.Owner.Username.ToLower(CultureInfo.InvariantCulture) ==
                        username.ToLower(CultureInfo.InvariantCulture))
                    .GroupBy(g => new
                    {
                        Type = g.Aircraft.Model.TypeName
                    })
                    .Select(s => new TotalsByType
                    {
                        Type = s.Key.Type,
                        TotalTimeSum = s.Sum(t => t.TotalFlightTime),
                        MultiSum = s.Sum(t => t.MultiEngine),
                        InstrumentSum = s.Sum(t => t.IMC),
                        NightSum = s.Sum(t => t.Night),
                        CrossCountrySum = s.Sum(t => t.CrossCountry),
                        TurbineSum = s.Sum(t => t.Turbine),
                        PICSum = s.Sum(t => t.PIC),
                        SICSum = s.Sum(t => t.SIC),
                        DualGivenSum = s.Sum(t => t.DualGiven),
                    }).ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
