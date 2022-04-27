using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;

namespace Nisshi.Requests.Vehicles
{
    public class Delete
    {
        public record Command(int id) : IRequest<Vehicle>;

        public class CommandHandler : BaseHandler, IRequestHandler<Command, Vehicle>
        {
            private readonly ICurrentUserAccessor accessor;
            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Vehicle> Handle(Command request, CancellationToken cancellationToken)
            {
                var username = accessor.GetCurrentUserName();

                var data = await context.Vehicles
                    .Include(x => x.Owner)
                    .FirstOrDefaultAsync(x => x.Id == request.id
                        && x.Owner.Username.ToLower(CultureInfo.InvariantCulture) == username.ToLower(CultureInfo.InvariantCulture), cancellationToken);

                if (data == null)
                    throw new DomainException(typeof(Vehicle), Message.ItemDoesNotExist);

                // Should also cascade to remove logbook entries
                context.Remove(data);
                await context.SaveChangesAsync();

                return data;
            }
        }
    }
}
