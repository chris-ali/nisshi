namespace Nisshi.Infrastructure
{
    public interface ICurrentUserAccessor
    {
        /// <summary>
        /// Gets the username of the currently logged in user
        /// </summary>
        /// <returns>Currently logged in username</returns>
        /// <exception cref="AuthenticationException">If no logged in user is found</exception>
        public string GetCurrentUserName();
    }
}
