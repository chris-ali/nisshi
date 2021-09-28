using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Nisshi.Requests.Models
{
    public class Update
    {
        public record Command(Model model) : IRequest<Model>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, Model>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Model> Handle(Command request, CancellationToken cancellationToken)
            {
                var data = await context.FindAsync<Model>(new object[] { request.model.ID }, cancellationToken);

                if (data == null) 
                {
                    var message = $"No Model found for id: {request.model.ID}";
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message});
                }

                var username = accessor.GetCurrentUserName();
                if (string.IsNullOrEmpty(username))
                {
                    var message = $"No logged in user found!";
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = message });
                }

                var user = await context.Users.Where(x => x.Username == username)
                    .FirstOrDefaultAsync(cancellationToken);

                var model = await context.Models.FindAsync(new object[] { data.ID }, cancellationToken);

                Update(ref data, request.model);
                data.DateUpdated = DateTime.Now;
                
                context.Update<Model>(data);
                await context.SaveChangesAsync(cancellationToken);

                return data;
            }

            /// <summary>
            /// Updates Model object from database with object in request
            /// </summary>
            /// <param name="toBeUpdated"></param>
            /// <param name="toUpdateWith"></param>
            private void Update(ref Model toBeUpdated, Model toUpdateWith) 
            {
                toBeUpdated.CategoryClass = toUpdateWith.CategoryClass;
            }
        }
    }
}