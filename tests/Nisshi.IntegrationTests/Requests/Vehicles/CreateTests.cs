using System;
using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Requests.Vehicles;
using Xunit;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.IntegrationTests.Requests.Vehicles
{
    /// <summary>
    /// Tests creating an vehicle in various scenarios
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
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var vehicleRequest = Helpers.CreateTestVehicle(user);

            var vehicleResponse = await fixture.SendAsync(new Create.Command(vehicleRequest));

            Assert.NotNull(vehicleResponse);

            var fromDb = await fixture.GetNisshiContext().Vehicles.FindAsync(vehicleResponse.Id);

            Assert.NotNull(fromDb);

            Assert.Equal(fromDb.Make, vehicleResponse.Make);
            Assert.Equal(fromDb.Miles, vehicleResponse.Miles);
            Assert.Equal(fromDb.Model, vehicleResponse.Model);
            Assert.Equal(fromDb.Notes, vehicleResponse.Notes);
            Assert.Equal(fromDb.RegistrationDue, vehicleResponse.RegistrationDue);
            Assert.Equal(fromDb.InspectionDue, vehicleResponse.InspectionDue);
            Assert.Equal(fromDb.LastRegistration, vehicleResponse.LastRegistration);
            Assert.Equal(fromDb.InspectionDue, vehicleResponse.InspectionDue);
            Assert.Equal(fromDb.Trim, vehicleResponse.Trim);
            Assert.Equal(fromDb.Year, vehicleResponse.Year);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Create.Command(null)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
