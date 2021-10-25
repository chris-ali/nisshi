using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Models;

namespace Nisshi.Requests.Models
{
    public class GetManyByPartialName
    {
        public record Query(string partialName) : IRequest<IList<Model>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<Model>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.partialName) || request.partialName.Length < 2)
                    return null;

                var data = await context.Models
                    .Where(x => x.ModelName
                        .ToLower(CultureInfo.InvariantCulture)
                        .StartsWith(request.partialName
                        .ToLower(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
