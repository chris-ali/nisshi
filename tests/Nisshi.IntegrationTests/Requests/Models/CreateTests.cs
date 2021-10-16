using System;
using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Requests.Models;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Models
{
    /// <summary>
    /// Tests creating a model in various scenarios
    /// </summary>
    public class CreateTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public CreateTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Should_Have_Been_Created()
        {
            var modelRequest = await Helpers.CreateTestModel(fixture);

            var modelResponse = await fixture.SendAsync(new Create.Command(modelRequest));

            Assert.NotNull(modelResponse);

            var fromDb = await fixture.GetNisshiContext().Models.FindAsync(modelResponse.Id);
            
            Assert.NotNull(fromDb);

            Assert.Equal(modelRequest.ModelName, modelResponse.ModelName);
            Assert.Equal(modelRequest.IsHighPerformance, modelResponse.IsHighPerformance);
            Assert.Equal(modelRequest.IsCertifiedSinglePilot, modelResponse.IsCertifiedSinglePilot);
            Assert.Equal(modelRequest.HasConstantPropeller, modelResponse.HasConstantPropeller);
            Assert.Equal(modelRequest.HasFlaps, modelResponse.HasFlaps);
            Assert.Equal(modelRequest.IsComplex, modelResponse.IsComplex);
            Assert.Equal(modelRequest.IsHelicopter, modelResponse.IsHelicopter);
            Assert.Equal(modelRequest.IsMotorGlider, modelResponse.IsMotorGlider);
            Assert.Equal(modelRequest.IsMultiEngine, modelResponse.IsMultiEngine);
            Assert.Equal(modelRequest.IsTailwheel, modelResponse.IsTailwheel);
            Assert.Equal(modelRequest.IsSimOnly, modelResponse.IsSimOnly);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Create.Command(null)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
