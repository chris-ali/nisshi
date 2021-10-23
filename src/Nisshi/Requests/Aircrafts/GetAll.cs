using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts
{
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
                    //.Include(x => x.Model)
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Username.ToUpper() == username.ToUpper())
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}