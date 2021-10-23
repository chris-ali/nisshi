using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Nisshi.Infrastructure.Errors;

namespace Nisshi.Infrastructure
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor accessor;

        public CurrentUserAccessor(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        /// <summary>
        /// Gets the currently logged in username
        /// </summary>
        /// <returns>Currently logged in username</returns>
        /// <exception cref="AuthenticationException">If no logged in user is found</exception>
        public string GetCurrentUserName()
        {
            string username = accessor.HttpContext?.User?.Claims?
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(username))
                throw new AuthenticationException(Message.NotLoggedIn.ToString());

            return username;
        }
    }
}
