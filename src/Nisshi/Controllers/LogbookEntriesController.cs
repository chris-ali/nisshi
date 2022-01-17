using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Nisshi.Infrastructure.Security;
using Nisshi.Models;
using Nisshi.Requests.LogbookEntries;

namespace Nisshi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes, Roles = "Administrator, User")]
    public class LogbookEntriesController : BaseNisshiController
    {
        public LogbookEntriesController(IMediator mediator) : base(mediator)
        {
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Filter, PageSize = 50)]
        [HttpGet("all")]
        public async Task<IEnumerable<LogbookEntry>> GetAll(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetAll.Query(), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<LogbookEntry> GetOne(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetOneById.Query(id), cancellationToken);
        }

        [HttpPut]
        public async Task<LogbookEntry> Update([FromBody] LogbookEntry logbookEntry, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Update.Command(logbookEntry), cancellationToken);
        }

        [HttpPost]
        public async Task<LogbookEntry> Create([FromBody] LogbookEntry logbookEntry, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(logbookEntry), cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<LogbookEntry> Delete(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Delete.Command(id), cancellationToken);
        }

        [HttpGet("analytics/totals/month")]
        public async Task<IEnumerable<TotalsByMonth>> GetTotalsByMonth(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetSumTotalsGroupByMonth.Query(), cancellationToken);
        }

        [HttpGet("analytics/totals/catclass")]
        public async Task<IEnumerable<TotalsByCategoryClass>> GetTotalsByCatClass(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetSumTotalsGroupByCatClass.Query(), cancellationToken);
        }

        [HttpGet("analytics/totals/instance")]
        public async Task<IEnumerable<TotalsByInstanceType>> GetTotalsByInstanceType(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetSumTotalsGroupByInstance.Query(), cancellationToken);
        }

        [HttpGet("analytics/totals/type")]
        public async Task<IEnumerable<TotalsByType>> GetTotalsByType(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetSumTotalsGroupByType.Query(), cancellationToken);
        }
    }
}
