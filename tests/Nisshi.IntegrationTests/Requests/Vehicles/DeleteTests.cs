using System;
using System.Threading.Tasks;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using Nisshi.Requests.Vehicles;
using Xunit;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.IntegrationTests.Requests.Vehicles
{
    /// <summary>
    /// Tests deleting an vehicle in various scenarios
    /// </summary>
    public class DeleteTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public DeleteTests()
        {
            this.fixture = new SliceFixture();
            ;
        }

        [Fact]
        public async Task Should_Have_Been_Deleted()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testVehicle = Helpers.CreateTestVehicle(user);
            var vehicleRequest = await Helpers.SaveAndGet<Vehicle>(fixture, testVehicle);

            var vehicleResponse = await fixture.SendAsync(new Delete.Command(vehicleRequest.Id));

            Assert.NotNull(vehicleResponse);

            vehicleResponse = await fixture.SendAsync(new GetOneById.Query(vehicleRequest.Id));

            Assert.Null(vehicleResponse);
        }

        [Fact]
        public async Task Should_Fail_Doesnt_Exist()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testVehicle = Helpers.CreateTestVehicle(user);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Delete.Command(testVehicle.Id)));
        }

        [Fact]
        public async Task Should_Not_Delete_Other_Users_Vehicle()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Delete.Command(1)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
