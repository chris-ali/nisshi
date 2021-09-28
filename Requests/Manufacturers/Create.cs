using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Nisshi.Requests.Manufacturers
{
    public class Create
    {
        public record Command(Manufacturer manufacturer) : IRequest<Manufacturer>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, Manufacturer>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Manufacturer> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.manufacturer == null) 
                {
                    var message = $"No manufacturer data found in request";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message });
                }

                var manufacturer = await context.Manufacturers
                    .Where(x => x.ManufacturerName == request.manufacturer.ManufacturerName)
                    .FirstOrDefaultAsync(cancellationToken);

                if (manufacturer != null)
                {
                    var message = $"{manufacturer.ManufacturerName} already exists";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message });
                }

                request.manufacturer.DateCreated = request.manufacturer.DateUpdated = DateTime.Now;

                await context.AddAsync<Manufacturer>(request.manufacturer, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.manufacturer;
            }
        }
    }
}