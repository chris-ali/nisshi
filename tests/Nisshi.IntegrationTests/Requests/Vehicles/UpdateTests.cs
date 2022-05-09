using System;
using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Enums;
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
    /// Tests editing an vehicle in various scenarios
    /// </summary>
    public class UpdateTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public UpdateTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Should_Have_Been_Updated()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testVehicle = Helpers.CreateTestVehicle(user);
            var vehicleRequest = await Helpers.SaveAndGet<Vehicle>(fixture, testVehicle);

            vehicleRequest.Make = "Honda";
            vehicleRequest.Miles = 120000;
            vehicleRequest.InspectionDue = DateTime.Today.AddDays(20);

            var vehicleResponse = await fixture.SendAsync(new Update.Command(vehicleRequest));

            Assert.NotNull(vehicleResponse);

            var fromDb = await fixture.GetNisshiContext().Vehicles.FindAsync(vehicleResponse.Id);

            Assert.NotNull(fromDb);

            Assert.Equal(fromDb.Make, vehicleResponse.Make);
            Assert.Equal(fromDb.Miles, vehicleResponse.Miles);
            Assert.Equal(fromDb.InspectionDue, vehicleResponse.InspectionDue);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Update.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_No_User()
        {
            var vehicleRequest = Helpers.CreateTestVehicle(null);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Update.Command(vehicleRequest)));
        }

        [Fact]
        public async Task Should_Fail_Doesnt_Exist()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testVehicle = Helpers.CreateTestVehicle(user);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Update.Command(testVehicle)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
