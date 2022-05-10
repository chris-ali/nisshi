using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Nisshi.Infrastructure.Security;
using Nisshi.Models;
using Nisshi.Requests.MaintenanceEntries;

namespace Nisshi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes, Roles = "Administrator, User")]
    public class MaintenanceEntriesController : BaseNisshiController
    {
        public MaintenanceEntriesController(IMediator mediator) : base(mediator)
        {
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Filter, PageSize = 50)]
        [HttpGet("all")]
        public async Task<IEnumerable<MaintenanceEntry>> GetAll(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetAll.Query(), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<MaintenanceEntry> GetOne(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetOneById.Query(id), cancellationToken);
        }

        [HttpPut]
        public async Task<MaintenanceEntry> Update([FromBody] MaintenanceEntry maintenanceEntry, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Update.Command(maintenanceEntry), cancellationToken);
        }

        [HttpPost]
        public async Task<MaintenanceEntry> Create([FromBody] MaintenanceEntry maintenanceEntry, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(maintenanceEntry), cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<MaintenanceEntry> Delete(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Delete.Command(id), cancellationToken);
        }
    }
}
