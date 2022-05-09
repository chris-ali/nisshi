using System;
using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Requests.MaintenanceEntries;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.MaintenanceEntries
{
    /// <summary>
    /// Tests creating a maintenance entry in various scenarios
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
            var maintenanceEntryRequest = await Helpers.CreateTestMaintenanceEntry(fixture, user);

            var maintenanceEntryResponse = await fixture.SendAsync(new Create.Command(maintenanceEntryRequest));

            Assert.NotNull(maintenanceEntryResponse);

            var fromDb = await fixture.GetNisshiContext().MaintenanceEntries.FindAsync(maintenanceEntryResponse.Id);

            Assert.NotNull(fromDb);

            Assert.Equal(fromDb.Comments, maintenanceEntryResponse.Comments);
            Assert.Equal(fromDb.DatePerformed, maintenanceEntryResponse.DatePerformed);
            Assert.Equal(fromDb.Duration, maintenanceEntryResponse.Duration);
            Assert.Equal(fromDb.MilesPerformed, maintenanceEntryResponse.MilesPerformed);
            Assert.Equal(fromDb.PerformedBy, maintenanceEntryResponse.PerformedBy);
            Assert.Equal(fromDb.RepairPrice, maintenanceEntryResponse.RepairPrice);
            Assert.Equal(fromDb.Type, maintenanceEntryResponse.Type);
            Assert.Equal(fromDb.WorkDescription, maintenanceEntryResponse.WorkDescription);
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
