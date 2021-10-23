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
                RuleFor(x => x.aircraft).NotNull().WithMessage(Message.NotNull.ToString());
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
                    throw new DomainException(typeof(Aircraft), Message.ItemDoesNotExist);

                var username = accessor.GetCurrentUserName();

                var user = await context.Users
                    .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

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
                toBeUpdated.TailNumber = toUpdateWith.TailNumber;
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
                toBeUpdated.Notes = toUpdateWith.Notes;
                toBeUpdated.IdModel = toUpdateWith.IdModel;
            }
        }
    }
}