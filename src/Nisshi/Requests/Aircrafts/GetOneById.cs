using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Models;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts
{
    public class GetOneById
    {
        public record Query(int id) : IRequest<Aircraft>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, Aircraft>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Aircraft> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.Aircraft
                    .Include(x => x.Owner)
                    .Include(x => x.Model)
                        .ThenInclude(x => x.Manufacturer)
                    .FirstOrDefaultAsync(x => x.Id == request.id
                        && x.Owner.Username.ToLower(CultureInfo.InvariantCulture) ==
                            username.ToLower(CultureInfo.InvariantCulture), cancellationToken);

                return data;
            }
        }
    }
}
