using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Nisshi.Models.Users;
using Nisshi.Requests.Users;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Users
{
    /// <summary>
    /// Tests authenticating a user in various scenarios
    /// </summary>
    public class AuthenticateTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public AuthenticateTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Should_Authenticate_User()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            var request = new Authenticate
            {
                Password = "Test123!",
                Username = Helpers.TestUserName
            };

            var response = await fixture.SendAsync(new Login.Command(request));

            Assert.NotNull(response);
            Assert.Equal(user.Username, response.Username);
            Assert.Equal(user.Email, response.Email);
            Assert.NotEmpty(response.Token);
        }

        [Fact]
        public async Task Should_Fail_Bad_Credentials()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            var request = new Authenticate
            {
                Password = "Test123456!",
                Username = Helpers.TestUserName
            };

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                fixture.SendAsync(new Login.Command(request)));
        }

        [Fact]
        public async Task Should_Fail_User_Doesnt_Exist()
        {
            var request = new Authenticate
            {
                Password = "Test123!",
                Username = Helpers.TestUserName
            };

            await Assert.ThrowsAsync<AuthenticationException>(() =>
                fixture.SendAsync(new Login.Command(request)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
