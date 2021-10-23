using System;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

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
                    .Where(x => x.ManufacturerName.ToUpper() == request.manufacturer.ManufacturerName.ToUpper())
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