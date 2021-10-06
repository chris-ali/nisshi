using Nisshi.Models;
using Nisshi.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Nisshi.Infrastructure.Security;

namespace Nisshi.Controllers
{
  public class UsersController : BaseNisshiController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<ActionResult<User>> GetCurrent(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetCurrent.Query(), cancellationToken);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<ActionResult<User>> UpdateProfile([FromBody] User user, CancellationToken cancellationToken)
        {
            return await mediator.Send(new UpdateProfile.Command(user), cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<UserLoggedIn>> Register([FromBody] UserRegistration registration, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Register.Command(registration), cancellationToken);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoggedIn>> Login([FromBody] UserLogin login, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Login.Command(login), cancellationToken);
        }
    }
}
