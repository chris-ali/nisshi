using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.CategoryClasses
{
  public class GetOneById
    {
        public record Query(int id) : IRequest<CategoryClass>;

        public class QueryHandler : BaseRequest, IRequestHandler<Query, CategoryClass>
        {
            public QueryHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<CategoryClass> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await context.CategoryClasses.FindAsync(new object[] { request.id }, cancellationToken);

                return data;
            }
        }
    }
}