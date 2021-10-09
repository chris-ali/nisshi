using Nisshi.Infrastructure;
using Nisshi.IntegrationTests.Requests;

namespace Nisshi.IntegrationTests
{
    public class StubCurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly string username;

        /// <summary>
        /// Provides a dummy username for integration tests
        /// </summary>
        public StubCurrentUserAccessor()
        {
            this.username = Helpers.TestUserName;
        }

        public string GetCurrentUserName()
        {
            return username;
        }
    }
}