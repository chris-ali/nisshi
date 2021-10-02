using Nisshi.Models;
using Nisshi.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Nisshi.Controllers
{
  public class UsersController : BaseNisshiController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<User>> Get(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetCurrent.Query(), cancellationToken);
        }

        [HttpPut]
        public async Task<ActionResult<User>> Update(User user, CancellationToken cancellationToken)
        {
            return await mediator.Send(new UpdateProfile.Command(user), cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(User user, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Create.Command(user), cancellationToken);
        }
    }
}
