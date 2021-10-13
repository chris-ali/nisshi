using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Requests.Manufacturers;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Manufacturers
{
    /// <summary>
    /// Tests creating a manufacturer in various scenarios
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
            var manufacturerRequest = Helpers.CreateTestManufacturer();

            var manufacturerResponse = await fixture.SendAsync(new Create.Command(manufacturerRequest));

            Assert.NotNull(manufacturerResponse);

            var fromDb = await fixture.GetNisshiContext().Manufacturers.FindAsync(manufacturerResponse.Id);
            
            Assert.NotNull(fromDb);

            Assert.Equal(fromDb.ManufacturerName, manufacturerResponse.ManufacturerName);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Create.Command(null)));
        }
    }
}
