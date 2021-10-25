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

namespace Nisshi.Requests.Airports
{
    public class GetManyByPartialCode
    {
        public record Query(string partialCode) : IRequest<IList<Airport>>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, IList<Airport>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<Airport>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.partialCode) || request.partialCode.Length < 3)
                    return null;

                var data = await context.Airports
                    .Where(x => x.AirportCode
                        .ToLower(CultureInfo.InvariantCulture)
                        .StartsWith(request.partialCode
                        .ToLower(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
