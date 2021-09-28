using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Nisshi.Requests.LogMessages
{
    public class Create
    {
        public record Command(LogMessage message) : IRequest<LogMessage>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, LogMessage>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<LogMessage> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.message == null)
                {
                    var message = $"No message data found in request";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message });
                }

                // Add server-side validation here?

                // Set current user as ownner
                var userName = accessor.GetCurrentUserName();
                var currentUser = await context.Users
                    .Where(x => x.Username == userName)
                    .FirstOrDefaultAsync(cancellationToken);

                if (currentUser == null)
                {
                    var message = $"No user found for username: {userName}";
                    // logger.LogWarning(message);
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = message });
                }

                request.message.DateCreated = DateTime.Now;
                request.message.Owner = currentUser;

                await context.AddAsync<LogMessage>(request.message, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                // logger.LogDebug($"Added new message: {request.message.Id} - {request.message.Contents}!");

                return request.message;
            }
        }
    }
}