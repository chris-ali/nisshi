using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using MediatR;
using System;
using FluentValidation;
using Nisshi.Models.Users;

namespace Nisshi.Requests.Users
{
  public class UpdateProfile
    {
        // TODO make user wrapper here that has password; Models.User should just have JWT token 

        public record Command(User user) : IRequest<User>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.user).NotNull().WithMessage($"User {Messages.NOT_NULL}");
            }
        }

        public class CommandHandler : BaseHandler, IRequestHandler<Command, User>
        {
            public CommandHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                var data = await context.FindAsync<User>(new object[] { request.user.Id }, cancellationToken);
                if (data == null) 
                {
                    var message = $"User: {request.user.Id} {Messages.DOES_NOT_EXIST}";
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message});
                }
                
                Update(ref data, request.user);
                data.DateUpdated = DateTime.Now;
                // TODO Handle password hashing here if password edited

                context.Update(data);
                await context.SaveChangesAsync(cancellationToken);

                return data;
            }

            /// <summary>
            /// Updates User object from database with object in request
            /// </summary>
            /// <param name="toBeUpdated"></param>
            /// <param name="toUpdateWith"></param>
            private void Update(ref User toBeUpdated, User toUpdateWith) 
            {
                toBeUpdated.CertificateNumber = toUpdateWith.CertificateNumber;
                toBeUpdated.CFIExpiration = toUpdateWith.CFIExpiration;
                toBeUpdated.FirstName = toUpdateWith.FirstName;
                toBeUpdated.IsInstructor = toUpdateWith.IsInstructor;
                toBeUpdated.LastBFR = toUpdateWith.LastBFR;
                toBeUpdated.LastMedical = toUpdateWith.LastMedical;
                toBeUpdated.LastName = toUpdateWith.LastName;
                toBeUpdated.License = toUpdateWith.License;
                toBeUpdated.MonthsToMedical = toUpdateWith.MonthsToMedical;
                toBeUpdated.Preferences = toUpdateWith.Preferences;
            }
        }
    }
}