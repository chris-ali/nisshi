using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Nisshi.Infrastructure;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using MediatR;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.Requests.Aircrafts
{
  public class Delete
    {
        public record Command(int id) : IRequest<Aircraft>;

        public class CommandHandler : BaseRequest, IRequestHandler<Command, Aircraft>
        {
            public CommandHandler(NisshiContext context) : base(context)
            {
            }

            public async Task<Aircraft> Handle(Command request, CancellationToken cancellationToken)
            {
                var data = await context.FindAsync<Aircraft>(new object[] { request.id }, cancellationToken);

                if (data == null) 
                {
                    var message = $"No aircraft found to delete for id: {request.id}";
                    throw new RestException(HttpStatusCode.NotFound, new { Message = message});
                }
                
                // Should also cascade to remove logbook entries
                context.Remove(data);
                await context.SaveChangesAsync();
                
                return data;
            }
        }
    }
}