namespace Nisshi.Infrastructure
{
    public interface ICurrentUserAccessor
    {
        /// <summary>
        /// Gets the username of the currently logged in user
        /// </summary>
        public string GetCurrentUserName();
    }
}