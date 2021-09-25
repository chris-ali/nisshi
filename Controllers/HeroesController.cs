using Nisshi.Models;
using Nisshi.Requests.Heroes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Nisshi.Controllers
{
  public class HeroesController : BaseNisshiController
    {
        public HeroesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IEnumerable<Hero>> GetAll(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetAll.Query(), cancellationToken);
        }

        /* [HttpGet("{createdBy}")]
        public async Task<IEnumerable<Hero>> GetManyByCreatedBy(string createdBy, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetManyByCreatedBy.Query(createdBy), cancellationToken);
        } */

        [HttpGet("{id}")]
        public async Task<Hero> GetOne(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetOneById.Query(id), cancellationToken);
        }

        [HttpPut]
        public async Task<Hero> Update(Hero hero, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Update.Command(hero), cancellationToken);
        }

        [HttpPost]
        public async Task<Hero> Create(Hero hero, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(hero), cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<Hero> Delete(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Delete.Command(id), cancellationToken);
        }
    }
}
