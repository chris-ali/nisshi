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

namespace Nisshi.Requests.Vehicles
{
    /// <summary>
    /// Gets all vehicles for the currently logged in user
    /// </summary>
    public class GetAll
    {
        public record Query() : IRequest<IList<Vehicle>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<Vehicle>>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<IList<Vehicle>> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.Vehicles
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Username.ToLower(CultureInfo.InvariantCulture) ==
                        username.ToLower(CultureInfo.InvariantCulture))
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
