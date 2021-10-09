using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Nisshi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseNisshiController : ControllerBase
    {
        protected readonly IMediator mediator;

        public BaseNisshiController(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}
