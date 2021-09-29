using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;

namespace Nisshi.Requests.Models
{
    public class Create
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
                if (request.model == null) 
                {
                    var message = $"No model data found in request";
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = message });
                }

                request.model.DateCreated = request.model.DateUpdated = DateTime.Now;

                await context.AddAsync<Model>(request.model, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.model;
            }
        }
    }
}