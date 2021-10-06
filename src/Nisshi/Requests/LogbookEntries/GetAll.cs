using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Nisshi.Requests.LogbookEntries 
{
    public class GetAll
    {
        public record Query() : IRequest<IList<LogbookEntry>>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, IList<LogbookEntry>>
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
                        .ThenInclude(x => x.Model)
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Username.ToUpper() == username.ToUpper())
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}