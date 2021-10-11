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
using FluentValidation;

namespace Nisshi.Requests.Airports
{
    public class Create
    {
        public record Command(Airport airport) : IRequest<Airport>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.airport).NotNull().WithMessage(Message.NotNull.ToString());
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, Airport>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Airport> Handle(Command request, CancellationToken cancellationToken)
            {
                var airport = await context.Airports
                    .Where(x => x.AirportCode.Contains(request.airport.AirportCode))
                    .FirstOrDefaultAsync(cancellationToken);

                if (airport != null)
                    throw new RestException(HttpStatusCode.BadRequest, Message.ItemExistsAlready);

                var username = accessor.GetCurrentUserName();
                if (string.IsNullOrEmpty(username))
                    throw new RestException(HttpStatusCode.Unauthorized, Message.NotLoggedIn);
                
                request.airport.DateCreated = request.airport.DateUpdated = DateTime.Now;
                request.airport.SourceUserName = username;

                await context.AddAsync<Airport>(request.airport, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.airport;
            }
        }
    }
}