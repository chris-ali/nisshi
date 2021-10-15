using System;
using System.Threading.Tasks;
using Nisshi.Infrastructure.Errors;
using Nisshi.Requests.Users;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Users
{
    /// <summary>
    /// Tests getting a user in various scenarios
    /// </summary>
    public class GetTests : IClassFixture<SliceFixture>, IDisposable
    {
        private readonly SliceFixture fixture;

        public GetTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Find_User()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            var response = await fixture.SendAsync(new GetCurrent.Query());

            Assert.NotNull(response);
            Assert.Equal(user.Username, response.Username);
            Assert.Equal(user.Email, response.Email);
        }

        [Fact]
        public async Task Should_Fail_Doesnt_Exist()
        {
            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new GetCurrent.Query()));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        } 
    }
}
