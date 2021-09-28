using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Nisshi.Requests.Users
{
  public class Update
    {
        // TODO make user wrapper here that has password; Models.User should just have JWT token 

        public record Command(User user) : IRequest<User>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, User>
        {
            public CommandHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                var data = await context.FindAsync<User>(new object[] { request.user.ID }, cancellationToken);

                if (data == null) 
                {
                    var message = $"No user found for id: {request.user.ID}";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message});
                }
                
                // logger.LogDebug($"Found user id: {data.Id} to update...");
                
                data.FirstName = request.user.FirstName;
                data.LastName = request.user.LastName;
                data.Email = request.user.Email;
                data.UserName = request.user.UserName;
                // TODO Handle password hashing here if password edited

                context.Update(data);
                await context.SaveChangesAsync(cancellationToken);

                // logger.LogDebug($"...updated successfully!");

                return data;
            }
        }
    }
}