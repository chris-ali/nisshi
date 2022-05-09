using System;
using System.Threading.Tasks;
using Nisshi.Models;
using Nisshi.Requests.Vehicles;
using Xunit;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.IntegrationTests.Requests.Vehicles
{
    /// <summary>
    /// Tests getting an vehicle in various scenarios
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
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testVehicle = Helpers.CreateTestVehicle(user);
            var vehicleRequest = await Helpers.SaveAndGet<Vehicle>(fixture, testVehicle);

            var vehicleResponse = await fixture.SendAsync(new GetOneById.Query(vehicleRequest.Id));

            Assert.NotNull(vehicleResponse);
            Assert.Equal(vehicleRequest.Make, vehicleResponse.Make);
            Assert.Equal(vehicleRequest.Miles, vehicleResponse.Miles);
            Assert.Equal(vehicleRequest.LastInspection, vehicleResponse.LastInspection);
        }

        [Fact]
        public async Task Should_Find_Three()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testVehicle = Helpers.CreateTestVehicle(user);
            var testVehicle1 = Helpers.CreateTestVehicle(user);
            var testVehicle2 = Helpers.CreateTestVehicle(user);

            await fixture.GetNisshiContext().Vehicles.AddRangeAsync(new[] { testVehicle, testVehicle1, testVehicle2 });
            await fixture.GetNisshiContext().SaveChangesAsync();

            var vehicleResponse = await fixture.SendAsync(new GetAll.Query());

            Assert.NotNull(vehicleResponse);
            Assert.Equal(3, vehicleResponse.Count);
        }

        [Fact]
        public async Task Should_Find_None()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var vehicleResponse = await fixture.SendAsync(new GetAll.Query());

            Assert.NotNull(vehicleResponse);
            Assert.Equal(0, vehicleResponse.Count);
        }

        [Fact]
        public async Task Should_Not_Find_Non_Existant_Vehicle()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var vehicleResponse = await fixture.SendAsync(new GetOneById.Query(11236));

            Assert.Null(vehicleResponse);
        }

        [Fact]
        public async Task Should_Not_Find_Other_Users_Vehicle()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var vehicleResponse = await fixture.SendAsync(new GetOneById.Query(1));

            Assert.Null(vehicleResponse);
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
