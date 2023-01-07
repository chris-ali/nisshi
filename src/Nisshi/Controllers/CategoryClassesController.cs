using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nisshi.Controllers.Interfaces;
using Nisshi.Models;
using Nisshi.Requests.CategoryClasses;

namespace Nisshi.Controllers
{
    public class CategoryClassesController : BaseNisshiController, ICategoryClassesController
    {
        public CategoryClassesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryClass>> GetAll(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetAll.Query(), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<CategoryClass> GetOneById(int id, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetOneById.Query(id), cancellationToken);
        }
    }
}
