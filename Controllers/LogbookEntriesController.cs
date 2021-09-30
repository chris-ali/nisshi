using Nisshi.Models;
using Nisshi.Requests.LogbookEntries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Nisshi.Controllers
{
    public class LogbookEntriesController : BaseNisshiController
    {
        public LogbookEntriesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("user/{username}")]
        public async Task<IEnumerable<LogbookEntry>> GetManyByUsername(string username, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetManyByUsername.Query(username), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<LogbookEntry> GetOne(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetOneById.Query(id), cancellationToken);
        }

        [HttpPut]
        public async Task<LogbookEntry> Update(LogbookEntry logbookEntry, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Update.Command(logbookEntry), cancellationToken);
        }

        [HttpPost]
        public async Task<LogbookEntry> Create(LogbookEntry logbookEntry, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(logbookEntry), cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<LogbookEntry> Delete(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Delete.Command(id), cancellationToken);
        }
    }
}
