using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts
{
    public class Delete
    {
        public record Command(int id) : IRequest<Aircraft>;

        public class CommandHandler : BaseHandler, IRequestHandler<Command, Aircraft>
        {
            private readonly ICurrentUserAccessor accessor;
            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Aircraft> Handle(Command request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.Aircraft
                    .Include(x => x.Owner)
                    .FirstOrDefaultAsync(x => x.Id == request.id
                        && x.Owner.Username.ToLower(CultureInfo.InvariantCulture) == username.ToLower(CultureInfo.InvariantCulture), cancellationToken);

                if (data == null)
                    throw new DomainException(typeof(Aircraft), Message.ItemDoesNotExist);

                // Should also cascade to remove logbook entries
                context.Remove(data);
                await context.SaveChangesAsync();

                return data;
            }
        }
    }
}
