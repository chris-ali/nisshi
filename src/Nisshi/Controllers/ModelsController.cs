﻿using Nisshi.Models;
using Nisshi.Requests.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Nisshi.Infrastructure.Security;

namespace Nisshi.Controllers
{
    public class ModelsController : BaseNisshiController
    {
        public ModelsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("search/{partialName}")]
        public async Task<IEnumerable<Model>> GetManyByPartialName(string partialName, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetManyByPartialName.Query(partialName), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<Model> GetOneById(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetOneById.Query(id), cancellationToken);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<Model> Create([FromBody] Model model, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(model), cancellationToken);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<Model> Update([FromBody] Model model, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Update.Command(model), cancellationToken);
        }
    }
}