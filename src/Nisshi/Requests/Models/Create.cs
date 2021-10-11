using System;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;
using FluentValidation;

namespace Nisshi.Requests.Models
{
    public class Create
    {
        public record Command(Model model) : IRequest<Model>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.model).NotNull().WithMessage(Message.NotNull.ToString());
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, Model>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Model> Handle(Command request, CancellationToken cancellationToken)
            {
                request.model.DateCreated = request.model.DateUpdated = DateTime.Now;

                await context.AddAsync<Model>(request.model, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return request.model;
            }
        }
    }
}