using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Requests.Models;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Models
{
    /// <summary>
    /// Tests creating a model in various scenarios
    /// </summary>
    public class CreateTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture fixture;

        public CreateTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Have_Been_Created()
        {
            var modelRequest = await Helpers.CreateTestModel(fixture);

            var modelResponse = await fixture.SendAsync(new Create.Command(modelRequest));

            Assert.NotNull(modelResponse);
            Assert.Equal(modelRequest.ModelName, modelResponse.ModelName);
            Assert.Equal(modelRequest.IsHighPerformance, modelResponse.IsHighPerformance);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Create.Command(null)));
        }
    }
}
