using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Nisshi.Requests.Heroes
{
    public class Update
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
                var data = await context.FindAsync<Hero>(new object[] { request.hero.Id }, cancellationToken);

                if (data == null) 
                {
                    var message = $"No hero found for id: {request.hero.Id}";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message});
                }

                var userName = accessor.GetCurrentUserName();

                if (string.IsNullOrEmpty(userName))
                {
                    var message = $"No logged in user found!";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = message });
                }
                
                // logger.LogDebug($"Found hero: {data.Id} - {data.Name} to update...");

                data.Name = request.hero.Name;
                data.RealName = request.hero.RealName;
                data.Power = request.hero.Power;
                
                context.Update<Hero>(data);
                await context.SaveChangesAsync(cancellationToken);

                // logger.LogDebug($"...updated successfully!");

                return data;
            }
        }
    }
}