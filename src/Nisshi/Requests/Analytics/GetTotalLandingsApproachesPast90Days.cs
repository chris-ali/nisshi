using System;
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
    public class GetTotalLandingsApproachesPast90Days
    {
        public record Query() : IRequest<LandingsApproaches>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, LandingsApproaches>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<LandingsApproaches> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = context.LogbookEntries
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Username.ToLower(CultureInfo.InvariantCulture) ==
                        username.ToLower(CultureInfo.InvariantCulture))
                    .Where(x => x.FlightDate >= DateTime.Today.AddDays(-90));

                return new LandingsApproaches
                {
                    DayLandings = await data.SumAsync(x => x.NumLandings, cancellationToken),
                    NightLandings = await data.SumAsync(x => x.NumNightLandings, cancellationToken),
                    Approaches = await data.SumAsync(x => x.NumInstrumentApproaches, cancellationToken),
                };
            }
        }
    }
}
