using System;
using System.Threading.Tasks;
using Nisshi.Models;
using Nisshi.Requests.Models;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Models
{
    /// <summary>
    /// Tests getting a model in various scenarios
    /// </summary>
    public class GetTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public GetTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Get_One_Should_Find_One()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testModel = await Helpers.CreateTestModel(fixture);
            var modelRequest = await Helpers.SaveAndGet<Model>(fixture, testModel);

            var modelResponse = await fixture.SendAsync(new GetOneById.Query(modelRequest.Id));

            Assert.NotNull(modelResponse);
            Assert.Equal(modelRequest.ModelName, modelResponse.ModelName);
            Assert.Equal(modelRequest.IsMultiEngine, modelResponse.IsMultiEngine);
        }

        [Fact]
        public async Task Get_One_Should_Find_None()
        {
            var modelResponse = await fixture.SendAsync(new GetOneById.Query(78945));

            Assert.Null(modelResponse);
        }

        [Fact]
        public async Task Get_Many_Should_Find_Two()
        {
            var aircraftResponse = await fixture.SendAsync(new GetManyByPartialName.Query("182"));

            Assert.NotNull(aircraftResponse);
            Assert.Equal(2, aircraftResponse.Count);
        }

        [Fact]
        public async Task Get_Many_Should_Find_None()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var aircraftResponse = await fixture.SendAsync(new GetManyByPartialName.Query("441"));

            Assert.NotNull(aircraftResponse);
            Assert.Equal(0, aircraftResponse.Count);
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
