using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts
{
    public class Create
    {
        public record Command(Aircraft aircraft) : IRequest<Aircraft>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, Aircraft>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Aircraft> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.aircraft == null) 
                {
                    var message = $"No aircraft data found in request";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message });
                }

                var username = accessor.GetCurrentUserName();

                if (string.IsNullOrEmpty(username))
                {
                    var message = $"No logged in user found!";
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = message });
                }

                var currentUser = await context.Users
                    .Where(x => x.Username == username)
                    .FirstOrDefaultAsync(cancellationToken);

                request.aircraft.DateCreated = request.aircraft.DateUpdated = DateTime.Now;
                request.aircraft.Owner = currentUser;

                await context.AddAsync<Aircraft>(request.aircraft, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.aircraft;
            }
        }
    }
}