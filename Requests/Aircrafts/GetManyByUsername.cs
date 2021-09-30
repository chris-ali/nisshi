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

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts 
{
    public class GetManyByUsername
    {
        public record Query(string username) : IRequest<IList<Aircraft>>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.username).NotNull().WithMessage($"Username {Messages.NOT_NULL}");
            }
        }

        public class QueryHandler : BaseRequest, IRequestHandler<Query, IList<Aircraft>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<Aircraft>> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.Aircraft
                    //.Include(x => x.Model)
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Username == request.username)
                    .ToListAsync(cancellationToken);
                
                return data;
            }
        }
    }
}