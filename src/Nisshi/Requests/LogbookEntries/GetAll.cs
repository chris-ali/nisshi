using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Models;

namespace Nisshi.Requests.LogbookEntries
{
    /// <summary>
    /// Gets all logbook entries for the currently logged in user
    /// </summary>
    public class GetAll
    {
        public record Query() : IRequest<IList<LogbookEntry>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<LogbookEntry>>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<IList<LogbookEntry>> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.LogbookEntries
                    .Include(x => x.Aircraft)
                    .Include(x => x.Aircraft.Model)
                    .Include(x => x.Aircraft.Model.Manufacturer)
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Username.ToLower(CultureInfo.InvariantCulture) ==
                        username.ToLower(CultureInfo.InvariantCulture))
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
