using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nisshi.Infrastructure.Security;
using Nisshi.Models;
using Nisshi.Requests.Manufacturers;

namespace Nisshi.Controllers
{
    public class ManufacturersController : BaseNisshiController
    {
        public ManufacturersController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<Manufacturer>> GetAll(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetAll.Query(), cancellationToken);
        }

        [HttpGet("{partialName}")]
        public async Task<IEnumerable<Manufacturer>> GetManyByPartialName(string partialName, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetManyByPartialName.Query(partialName), cancellationToken);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes, Roles = "Administrator, User")]
        public async Task<Manufacturer> Create([FromBody] Manufacturer manufacturer, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(manufacturer), cancellationToken);
        }
    }
}
