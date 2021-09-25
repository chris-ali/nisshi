using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Nisshi.Requests.Heroes
{
  public class Delete
    {
        public record Command(int id) : IRequest<Hero>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, Hero>
        {
            public CommandHandler(HeroesDbContext context) : base(context)
            {
            }

            public async Task<Hero> Handle(Command request, CancellationToken cancellationToken)
            {
                var data = await context.FindAsync<Hero>(new object[] { request.id }, cancellationToken);

                if (data == null) 
                {
                    var message = $"No hero found to delete for id: {request.id}";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message});
                }
                
                // logger.LogDebug($"Found hero: {data.Id} - {data.Name} to delete...");

                context.Remove(data);
                await context.SaveChangesAsync();
                
                // logger.LogDebug($"...deleted successfully!");

                return data;
            }
        }
    }
}