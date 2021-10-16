using System;
using System.Threading.Tasks;
using Nisshi.Requests.Manufacturers;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Manufacturers
{
    /// <summary>
    /// Tests getting a manufacturer in various scenarios
    /// </summary>
    public class GetTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public GetTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Should_Find_One()
        {
            var manufacturerResponse = await fixture.SendAsync(new GetManyByPartialName.Query("Ces"));

            Assert.NotNull(manufacturerResponse);
            Assert.Equal(1, manufacturerResponse.Count);
        }

        [Fact]
        public async Task Should_Find_None()
        {
            var manufacturerResponse = await fixture.SendAsync(new GetManyByPartialName.Query("Nothing"));

            Assert.NotNull(manufacturerResponse);
            Assert.Equal(0, manufacturerResponse.Count);
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
