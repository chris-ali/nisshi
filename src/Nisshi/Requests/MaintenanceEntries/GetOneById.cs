using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Models;

namespace Nisshi.Requests.MaintenanceEntries
{
    public class GetOneById
    {
        public record Query(int id) : IRequest<MaintenanceEntry>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, MaintenanceEntry>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<MaintenanceEntry> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.MaintenanceEntries
                    .Include(x => x.Owner)
                    .Include(x => x.Vehicle)
                    .FirstOrDefaultAsync(x => x.Id == request.id
                        && x.Owner.Username.ToLower(CultureInfo.InvariantCulture) ==
                            username.ToLower(CultureInfo.InvariantCulture), cancellationToken);

                return data;
            }
        }
    }
}
