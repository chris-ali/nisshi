using System;
using System.Threading.Tasks;
using Nisshi.Requests.Airports;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Airports
{
    /// <summary>
    /// Tests getting an airport in various scenarios
    /// </summary>
    public class GetTests : IClassFixture<SliceFixture>, IDisposable
    {
        private readonly SliceFixture fixture;

        public GetTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Find_One()
        {
            var airportResponse = await fixture.SendAsync(new GetManyByPartialCode.Query("KTT"));

            Assert.NotNull(airportResponse);
            Assert.Equal(1, airportResponse.Count);
        }

        [Fact]
        public async Task Should_Find_None()
        {
            var airportResponse = await fixture.SendAsync(new GetManyByPartialCode.Query("Nothing"));

            Assert.NotNull(airportResponse);
            Assert.Equal(0, airportResponse.Count);
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
