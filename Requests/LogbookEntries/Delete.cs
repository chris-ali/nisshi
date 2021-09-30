using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;

namespace Nisshi.Requests.LogbookEntries
{
  public class Delete
    {
        public record Command(int id) : IRequest<LogbookEntry>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, LogbookEntry>
        {
            public CommandHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<LogbookEntry> Handle(Command request, CancellationToken cancellationToken)
            {
                var data = await context.FindAsync<LogbookEntry>(new object[] { request.id }, cancellationToken);

                if (data == null) 
                {
                    var message = $"Logbook Entry: {request.id} {Messages.DOES_NOT_EXIST}";
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message});
                }
                
                context.Remove(data);
                await context.SaveChangesAsync();
                
                return data;
            }
        }
    }
}