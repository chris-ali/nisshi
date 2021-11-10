using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nisshi.Infrastructure.Security;
using Nisshi.Models.Users;
using Nisshi.Requests.Users;

namespace Nisshi.Controllers
{
    public class UsersController : BaseNisshiController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes, Roles = "Administrator, User")]
        public async Task<ActionResult<User>> GetCurrent(CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetCurrent.Query(), cancellationToken);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes, Roles = "Administrator, User")]
        public async Task<ActionResult<User>> UpdateProfile([FromBody] Profile edit, CancellationToken cancellationToken)
        {
            return await mediator.Send(new UpdateProfile.Command(edit), cancellationToken);
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoggedIn>> Register([FromBody] Registration registration, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Register.Command(registration), cancellationToken);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoggedIn>> Login([FromBody] Models.Users.Authenticate authenticate, CancellationToken cancellationToken)
        {
            return await mediator.Send(new Requests.Users.Login.Command(authenticate), cancellationToken);
        }
    }
}
