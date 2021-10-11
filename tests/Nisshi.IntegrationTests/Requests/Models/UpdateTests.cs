using System;
using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using Nisshi.Requests.Models;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Models
{
    /// <summary>
    /// Tests editing a model in various scenarios
    /// </summary>
    public class UpdateTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture fixture;

        public UpdateTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Have_Been_Updated()
        {
            var testModel = await Helpers.CreateTestModel(fixture);
            var modelRequest = await Helpers.SaveAndGet<Model>(fixture, testModel);

            modelRequest.ModelName = "New Model Name";
            modelRequest.HasFlaps = false;
            
            var modelResponse = await fixture.SendAsync(new Update.Command(modelRequest));

            Assert.NotNull(modelResponse);
            Assert.Equal(modelRequest.ModelName, modelResponse.ModelName);
            Assert.Equal(modelRequest.HasFlaps, modelResponse.HasFlaps);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Update.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_Doesnt_Exist()
        {
            var testModel = await Helpers.CreateTestModel(fixture);

            await Assert.ThrowsAsync<RestException>(() => fixture.SendAsync(new Update.Command(testModel)));
        }
    }
}
