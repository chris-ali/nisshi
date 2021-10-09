using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Nisshi.Infrastructure
{
  public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor accessor;

        public CurrentUserAccessor(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;   
        }
        
        public string GetCurrentUserName()
        {
            return accessor.HttpContext?.User?.Claims?
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}