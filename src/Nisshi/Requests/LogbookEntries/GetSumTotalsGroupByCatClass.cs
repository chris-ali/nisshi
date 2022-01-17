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
    public class GetSumTotalsGroupByCatClass
    {
        public record Query() : IRequest<IList<TotalsByCategoryClass>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<TotalsByCategoryClass>>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<IList<TotalsByCategoryClass>> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.LogbookEntries
                    .Include(x => x.Owner)
                    .Include(x => x.Aircraft)
                        .ThenInclude(x => x.Model)
                        .ThenInclude(x => x.CategoryClass)
                    .Where(x => x.Owner.Username.ToLower(CultureInfo.InvariantCulture) ==
                        username.ToLower(CultureInfo.InvariantCulture))
                    .GroupBy(g => new {
                        CategoryClass = g.Aircraft.Model.CategoryClass.CatClass
                    })
                    .Select(s => new TotalsByCategoryClass {
                        CategoryClass = s.Key.CategoryClass,
                        TotalTimeSum = s.Sum(t => t.TotalFlightTime),
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
