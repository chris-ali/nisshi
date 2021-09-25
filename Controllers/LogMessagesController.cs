using Nisshi.Models;
using Nisshi.Requests.LogMessages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Nisshi.Controllers
{
  public class LogMessagesController : BaseNisshiController
    {
        public LogMessagesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IEnumerable<LogMessage>> GetManyByUserName(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetMany.Query(), cancellationToken);
        }

        [HttpPost]
        public async Task<LogMessage> Create(LogMessage message, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(message), cancellationToken);
        }

        [HttpDelete]
        public async Task<IEnumerable<LogMessage>> DeleteManyByUserName(CancellationToken cancellationToken)
        {
            return await mediator.Send(new DeleteMany.Command(), cancellationToken);
        }
    }
}
