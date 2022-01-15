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
    public class GetSumTotalTimeGroupByCatClass
    {
        public record Query() : IRequest<IList<TotalTimeByCategoryClass>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<TotalTimeByCategoryClass>>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<IList<TotalTimeByCategoryClass>> Handle(Query request, CancellationToken cancellationToken)
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
                    .Select(s => new TotalTimeByCategoryClass {
                        CategoryClass = s.Key.CategoryClass,
                        TotalTimeSum = s.Sum(t => t.TotalFlightTime)
                    }).ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
