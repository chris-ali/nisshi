using System;
using System.Threading.Tasks;
using Nisshi.Infrastructure.Errors;
using Nisshi.Requests.Users;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Users
{
    /// <summary>
    /// Tests refreshing a JWT in various scenarios
    /// </summary>
    public class RefreshTokenTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public RefreshTokenTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Should_Refresh_Token()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            var response = await fixture.SendAsync(new RefreshToken.Command());

            Assert.NotNull(response);
            Assert.Equal(user.Username, response.Username);
            Assert.Equal(user.Email, response.Email);
            Assert.NotEmpty(response.Token);
        }

        [Fact]
        public async Task Should_Fail_User_Doesnt_Exist()
        {
            await Assert.ThrowsAsync<DomainException>(() =>
                fixture.SendAsync(new RefreshToken.Command()));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
