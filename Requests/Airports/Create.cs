using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Nisshi.Requests.Airports
{
    public class Create
    {
        public record Command(Airport airport) : IRequest<Airport>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, Airport>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Airport> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.airport == null) 
                {
                    var message = $"No airport data found in request";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message });
                }

                var airport = await context.Airports
                    .Where(x => x.AirportCode.Contains(request.airport.AirportCode))
                    .FirstOrDefaultAsync(cancellationToken);

                if (airport != null)
                {
                    var message = $"{airport.AirportCode} already exists";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message });
                }

                var username = accessor.GetCurrentUserName();

                if (string.IsNullOrEmpty(username))
                {
                    var message = $"No logged in user found!";
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = message });
                }

                request.airport.DateCreated = request.airport.DateUpdated = DateTime.Now;
                request.airport.SourceUserName = username;

                await context.AddAsync<Airport>(request.airport, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.airport;
            }
        }
    }
}