using System;
using System.Threading.Tasks;
using Nisshi.Requests.CategoryClasses;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.CategoryClasses
{
    /// <summary>
    /// Tests getting a categoryclass in various scenarios
    /// </summary>
    public class GetTests : IClassFixture<SliceFixture>, IDisposable
    {
        private readonly SliceFixture fixture;

        public GetTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Get_All_Should_Find_Two()
        {
            var catClassResponse = await fixture.SendAsync(new GetAll.Query());

            Assert.NotNull(catClassResponse);
            Assert.Equal(2, catClassResponse.Count);
        }

        [Fact]
        public async Task Get_One_Should_Find_One()
        {
            var catClassResponse = await fixture.SendAsync(new GetOneById.Query(1));

            Assert.NotNull(catClassResponse);
        }

        [Fact]
        public async Task Get_One_Should_Find_None()
        {
            var catClassResponse = await fixture.SendAsync(new GetOneById.Query(465));

            Assert.Null(catClassResponse);
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
