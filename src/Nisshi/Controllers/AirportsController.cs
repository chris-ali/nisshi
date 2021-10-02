using Nisshi.Models;
using Nisshi.Requests.Airports;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Nisshi.Controllers
{
    public class AirportsController : BaseNisshiController
    {
        public AirportsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{partialCode}")]
        public async Task<IEnumerable<Airport>> GetManyByPartialCode(string partialCode, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetManyByPartialCode.Query(partialCode), cancellationToken);
        }

        [HttpPost]
        public async Task<Airport> Create(Airport aircraft, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(aircraft), cancellationToken);
        }
    }
}
