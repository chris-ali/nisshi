using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;

namespace Nisshi.Requests.Manufacturers
{
    public class Create
    {
        public record Command(Manufacturer manufacturer) : IRequest<Manufacturer>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.manufacturer).NotNull().WithMessage(Message.NotNull.ToString());
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, Manufacturer>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Manufacturer> Handle(Command request, CancellationToken cancellationToken)
            {
                var manufacturer = await context.Manufacturers
                    .Where(x => x.ManufacturerName.ToLower(CultureInfo.InvariantCulture) == request.manufacturer.ManufacturerName.ToLower(CultureInfo.InvariantCulture))
                    .FirstOrDefaultAsync(cancellationToken);

                if (manufacturer != null)
                    throw new DomainException(typeof(Manufacturer), Message.ItemExistsAlready);

                request.manufacturer.DateCreated = request.manufacturer.DateUpdated = DateTime.Now;

                await context.AddAsync<Manufacturer>(request.manufacturer, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.manufacturer;
            }
        }
    }
}
