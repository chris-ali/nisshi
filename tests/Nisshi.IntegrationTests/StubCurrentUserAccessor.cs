using Nisshi.Infrastructure;

namespace Nisshi.IntegrationTests
{
    public class StubCurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly string username;

        /// <summary>
        /// Provides a dummy username for integration tests
        /// </summary>
        /// <param name="username"></param>
        public StubCurrentUserAccessor(string username)
        {
            this.username = username;
        }

        public string GetCurrentUserName()
        {
            return username;
        }
    }
}