using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Nisshi.Infrastructure.Security;
using Nisshi.Models;
using Nisshi.Requests.Vehicles;

namespace Nisshi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes, Roles = "Administrator, User")]
    public class VehicleController : BaseNisshiController
    {
        public VehicleController(IMediator mediator) : base(mediator)
        {
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Filter, PageSize = 50)]
        [HttpGet("all")]
        public async Task<IEnumerable<Vehicle>> GetAll(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetAll.Query(), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<Vehicle> GetOne(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetOneById.Query(id), cancellationToken);
        }

        [HttpPut]
        public async Task<Vehicle> Update([FromBody] Vehicle vehicle, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Update.Command(vehicle), cancellationToken);
        }

        [HttpPost]
        public async Task<Vehicle> Create([FromBody] Vehicle vehicle, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(vehicle), cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<Vehicle> Delete(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Delete.Command(id), cancellationToken);
        }
    }
}
