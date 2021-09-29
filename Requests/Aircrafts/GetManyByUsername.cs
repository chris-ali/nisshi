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

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts 
{
    public class GetManyByUsername
    {
        public record Query(string username) : IRequest<IList<Aircraft>>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, IList<Aircraft>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<Aircraft>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.username))
                {
                    var message = $"Must provide username";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message });
                }
                
                var data = await context.Aircraft
                    //.Include(x => x.Model)
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Username == request.username)
                    .ToListAsync(cancellationToken);
                
                if (data.Count == 0)
                {
                    var message = $"No aircraft found for user: {request.username}";
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message });
                }
                
                return data;
            }
        }
    }
}