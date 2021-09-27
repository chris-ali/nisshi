using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Nisshi.Requests.Heroes
{
    public class Create
    {
        public record Command(Hero hero) : IRequest<Hero>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, Hero>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Hero> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.hero == null) 
                {
                    var message = $"No hero data found in request";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message});
                }

                var userName = accessor.GetCurrentUserName();

                if (string.IsNullOrEmpty(userName))
                {
                    var message = $"No logged in user found!";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = message });
                }

                // Add server-side validation here?

                request.hero.DateCreated = request.hero.DateUpdated = DateTime.Now;

                await context.AddAsync<Hero>(request.hero, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                // logger.LogDebug($"Added new hero: {request.hero.Id} - {request.hero.Name}!");

                return request.hero;
            }
        }
    }
}