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

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts
{
    /// <summary>
    /// Gets all aircraft for the currently logged in user
    /// </summary>
    public class GetAll
    {
        public record Query() : IRequest<IList<Aircraft>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<Aircraft>>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<IList<Aircraft>> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.Aircraft
                    .Include(x => x.Owner)
                    .Include(x => x.Model)
                        .ThenInclude(x => x.Manufacturer)
                    .Include(x => x.Model.CategoryClass)
                    .Where(x => x.Owner.Username.ToLower(CultureInfo.InvariantCulture) ==
                        username.ToLower(CultureInfo.InvariantCulture))
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
