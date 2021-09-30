using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Nisshi.Requests.Models
{
    public class Update
    {
        public record Command(Model model) : IRequest<Model>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.model).NotNull().WithMessage($"Model {Messages.NOT_NULL}");
            }
        }

        public class CommandHandler : BaseRequest, IRequestHandler<Command, Model>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Model> Handle(Command request, CancellationToken cancellationToken)
            {
                var data = await context.FindAsync<Model>(new object[] { request.model.Id }, cancellationToken);

                if (data == null) 
                {
                    var message = $"Model: {request.model.Id} {Messages.DOES_NOT_EXIST}";
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message});
                }

                var model = await context.Models.FindAsync(new object[] { data.Id }, cancellationToken);

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
                toBeUpdated.Family = toUpdateWith.Family;
                toBeUpdated.HasConstantPropeller = toUpdateWith.HasConstantPropeller;
                toBeUpdated.HasFlaps = toUpdateWith.HasFlaps;
                toBeUpdated.IdCategoryClass = toUpdateWith.IdCategoryClass;
                toBeUpdated.IdManufacturer = toUpdateWith.IdManufacturer;
                toBeUpdated.IsCertifiedSinglePilot = toUpdateWith.IsCertifiedSinglePilot;
                toBeUpdated.IsComplex = toUpdateWith.IsComplex;
                toBeUpdated.IsHelicopter = toUpdateWith.IsHelicopter;
                toBeUpdated.IsHighPerformance = toUpdateWith.IsHighPerformance;
                toBeUpdated.IsMotorGlider = toUpdateWith.IsMotorGlider;
                toBeUpdated.IsMultiEngine = toUpdateWith.IsMultiEngine;
                toBeUpdated.IsSimOnly = toUpdateWith.IsSimOnly;
                toBeUpdated.IsTailwheel = toUpdateWith.IsTailwheel;
                toBeUpdated.IsTurbine = toUpdateWith.IsTurbine;
            }
        }
    }
}