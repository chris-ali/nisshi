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

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts
{
    public class Update
    {
        public record Command(Aircraft aircraft) : IRequest<Aircraft>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.aircraft).NotNull().WithMessage($"Aircraft {Messages.NOT_NULL}");
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, Aircraft>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<Aircraft> Handle(Command request, CancellationToken cancellationToken)
            {
                var data = await context.FindAsync<Aircraft>(new object[] { request.aircraft.Id }, cancellationToken);
                if (data == null) 
                {
                    var message = $"Aircraft: {request.aircraft.Id} {Messages.DOES_NOT_EXIST}";
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message});
                }

                var username = accessor.GetCurrentUserName();
                
                if (string.IsNullOrEmpty(username))
                    throw new RestException(HttpStatusCode.Unauthorized, new { Message = Messages.NOT_LOGGED_IN });
                
                var user = await context.Users.Where(x => x.Username == username)
                    .FirstOrDefaultAsync(cancellationToken);

                Update(ref data, request.aircraft);
                data.DateUpdated = DateTime.Now;
                data.Owner = user;
                
                context.Update<Aircraft>(data);
                await context.SaveChangesAsync(cancellationToken);

                return data;
            }

            /// <summary>
            /// Updates Aircraft object from database with object in request
            /// </summary>
            /// <param name="toBeUpdated"></param>
            /// <param name="toUpdateWith"></param>
            private void Update(ref Aircraft toBeUpdated, Aircraft toUpdateWith) 
            {
                toBeUpdated.InstanceType = toUpdateWith.InstanceType;
                toBeUpdated.Last100Hobbs = toUpdateWith.Last100Hobbs;
                toBeUpdated.LastAltimeter = toUpdateWith.LastAltimeter;
                toBeUpdated.LastAnnual = toUpdateWith.LastAnnual;
                toBeUpdated.LastELT = toUpdateWith.LastELT;
                toBeUpdated.LastEngineHobbs = toUpdateWith.LastEngineHobbs;
                toBeUpdated.LastOilHobbs = toUpdateWith.LastOilHobbs;
                toBeUpdated.LastPitotStatic = toUpdateWith.LastPitotStatic;
                toBeUpdated.LastTransponder = toUpdateWith.LastTransponder;
                toBeUpdated.LastVOR = toUpdateWith.LastVOR;
                toBeUpdated.IdModel = toUpdateWith.IdModel;
            }
        }
    }
}