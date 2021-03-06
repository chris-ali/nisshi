using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models.Users;

namespace Nisshi.Requests.Users
{
    public class GetCurrent
    {
        public record Query() : IRequest<User>;

        public class QueryHandler : BaseHandler, IRequestHandler<Query, User>
        {
            private readonly ICurrentUserAccessor accessor;

            public QueryHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.Users
                    .SingleOrDefaultAsync(x => x.Username == username, cancellationToken);

                if (data == null)
                    throw new DomainException(typeof(User), Message.ItemDoesNotExist);

                return data;
            }
        }
    }
}
