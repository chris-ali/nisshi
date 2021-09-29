﻿using Nisshi.Models;
using Nisshi.Requests.Manufacturers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Nisshi.Controllers
{
    public class ManufacturersController : BaseNisshiController
    {
        public ManufacturersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("{partialName}")]
        public async Task<IEnumerable<Manufacturer>> GetManyByPartialName(string partialName, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetManyByPartialName.Query(partialName), cancellationToken);
        }

        [HttpPost]
        public async Task<Manufacturer> Create(Manufacturer manufacturer, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(manufacturer), cancellationToken);
        }
    }
}
