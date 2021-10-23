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

namespace Nisshi.Requests.Manufacturers
{
    public class GetManyByPartialName
    {
        public record Query(string partialName) : IRequest<IList<Manufacturer>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<Manufacturer>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<Manufacturer>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.partialName) || request.partialName.Length < 3)
                    return null;

                var data = await context.Manufacturers
                    .Where(x => x.ManufacturerName
                        .ToLower(CultureInfo.InvariantCulture)
                        .StartsWith(request.partialName
                        .ToLower(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
