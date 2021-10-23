using System;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Nisshi.Requests.LogbookEntries
{
    public class Update
    {
        public record Command(LogbookEntry logbookEntry) : IRequest<LogbookEntry>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.logbookEntry).NotNull().WithMessage(Message.NotNull.ToString());
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, LogbookEntry>
        {
            private readonly ICurrentUserAccessor accessor;

            public CommandHandler(NisshiContext context, ICurrentUserAccessor accessor) : base(context)
            {
                this.accessor = accessor;
            }

            public async Task<LogbookEntry> Handle(Command request, CancellationToken cancellationToken)
            {
                var data = await context.FindAsync<LogbookEntry>(new object[] { request.logbookEntry.Id }, cancellationToken);

                if (data == null)
                    throw new DomainException(typeof(LogbookEntry), Message.ItemDoesNotExist);

                var username = accessor.GetCurrentUserName();

                var user = await context.Users
                    .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

                Update(ref data, request.logbookEntry);
                data.DateUpdated = DateTime.Now;
                data.Owner = user;

                context.Update<LogbookEntry>(data);
                await context.SaveChangesAsync(cancellationToken);

                return data;
            }

            /// <summary>
            /// Updates LogbookEntry object from database with object in request
            /// </summary>
            /// <param name="toBeUpdated"></param>
            /// <param name="toUpdateWith"></param>
            private void Update(ref LogbookEntry toBeUpdated, LogbookEntry toUpdateWith)
            {
                toBeUpdated.Comments = toUpdateWith.Comments;
                toBeUpdated.CrossCountry = toUpdateWith.CrossCountry;
                toBeUpdated.DualGiven = toUpdateWith.DualGiven;
                toBeUpdated.DualReceived = toUpdateWith.DualReceived;
                toBeUpdated.FlightDate = toUpdateWith.FlightDate;
                toBeUpdated.GroundSim = toUpdateWith.GroundSim;
                toBeUpdated.HobbsEnd = toUpdateWith.HobbsEnd;
                toBeUpdated.HobbsStart = toUpdateWith.HobbsStart;
                toBeUpdated.IMC = toUpdateWith.IMC;
                toBeUpdated.MultiEngine = toUpdateWith.MultiEngine;
                toBeUpdated.Night = toUpdateWith.Night;
                toBeUpdated.NumFullStopLandings = toUpdateWith.NumFullStopLandings;
                toBeUpdated.NumInstrumentApproaches = toUpdateWith.NumInstrumentApproaches;
                toBeUpdated.NumLandings = toUpdateWith.NumLandings;
                toBeUpdated.NumNightLandings = toUpdateWith.NumNightLandings;
                toBeUpdated.PIC = toUpdateWith.PIC;
                toBeUpdated.Route = toUpdateWith.Route;
                toBeUpdated.SIC = toUpdateWith.SIC;
                toBeUpdated.SimulatedInstrument = toUpdateWith.SimulatedInstrument;
                toBeUpdated.TotalFlightTime = toUpdateWith.TotalFlightTime;
                toBeUpdated.IdAircraft = toUpdateWith.IdAircraft;
            }
        }
    }
}