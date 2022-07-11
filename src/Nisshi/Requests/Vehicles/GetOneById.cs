using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Models;

namespace Nisshi.Requests.Vehicles
{
    public class GetOneById
    {
        public record Query(int id) : IRequest<Vehicle>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, Vehicle>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Vehicle> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.Vehicles
                    .Include(x => x.Owner)
                    .FirstOrDefaultAsync(x => x.Id == request.id
                        && x.Owner.Username.ToLower(CultureInfo.InvariantCulture) ==
                            username.ToLower(CultureInfo.InvariantCulture), cancellationToken);

                return data;
            }
        }
    }
}
