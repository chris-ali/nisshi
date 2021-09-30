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
    public class GetManyByUsername
    {
        public record Query(string username) : IRequest<IList<LogbookEntry>>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.username).NotNull().WithMessage($"Username {Messages.NOT_NULL}");
            }
        }

        public class QueryHandler : BaseRequest, IRequestHandler<Query, IList<LogbookEntry>>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<IList<LogbookEntry>> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.LogbookEntries
                    .Include(x => x.Aircraft)
                        .ThenInclude(x => x.Model)
                    .Include(x => x.Owner)
                    .Where(x => x.Owner.Username == request.username)
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}