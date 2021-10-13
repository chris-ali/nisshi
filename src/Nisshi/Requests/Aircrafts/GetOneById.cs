using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                    .FirstOrDefaultAsync(x => x.Id == request.id 
                        && x.Owner.Username.ToUpper() == username.ToUpper(), cancellationToken);

                return data;
            }
        }
    }
}