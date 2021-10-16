using Nisshi.Models;
using Nisshi.Requests.Aircrafts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Nisshi.Infrastructure.Security;

namespace Nisshi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes, Roles = "Administrator, User")]
    public class AircraftController : BaseNisshiController
    {
        public AircraftController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("user")]
        public async Task<IEnumerable<Aircraft>> GetAll(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetAll.Query(), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<Aircraft> GetOne(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetOneById.Query(id), cancellationToken);
        }

        [HttpPut]
        public async Task<Aircraft> Update([FromBody] Aircraft aircraft, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Update.Command(aircraft), cancellationToken);
        }

        [HttpPost]
        public async Task<Aircraft> Create([FromBody] Aircraft aircraft, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(aircraft), cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<Aircraft> Delete(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Delete.Command(id), cancellationToken);
        }
    }
}
