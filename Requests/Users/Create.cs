using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Nisshi.Requests.Users
{
    public class Create
    {
        // TODO make user wrapper here that has reistration details; use as input to endpoint; Models.User should just have JWT token

        public record Command(User user) : IRequest<User>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, User>
        {
            public CommandHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.user == null) 
                {
                    var message = $"No user data found in request";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message});
                }

                if (await context.Users.Where(x => x.Username == request.user.Username).AnyAsync(cancellationToken))
                {
                    var message = $"Username already exists in database";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message});
                }

                if (await context.Users.Where(x => x.Email == request.user.Email).AnyAsync(cancellationToken))
                {
                    var message = $"Email already exists in database";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message});
                }

                // Add server-side validation here?

                // TODO Create token hash and salt here

                await context.AddAsync<User>(request.user);
                await context.SaveChangesAsync(cancellationToken);

                return request.user;
            }
        }
    }
}