using System;
using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using Nisshi.Requests.MaintenanceEntries;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.MaintenanceEntries
{
    /// <summary>
    /// Tests editing a maintenance entry in various scenarios
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
            var testMaintenanceEntry = await Helpers.CreateTestMaintenanceEntry(fixture, user);
            var maintenanceEntryRequest = await Helpers.SaveAndGet<MaintenanceEntry>(fixture, testMaintenanceEntry);

            maintenanceEntryRequest.Comments = "New test comments.";
            maintenanceEntryRequest.RepairPrice = 200;
            maintenanceEntryRequest.DatePerformed = DateTime.Today.AddMonths(-12);
            maintenanceEntryRequest.Type = Infrastructure.Enums.MaintenanceType.Preventative;

            var maintenanceEntryResponse = await fixture.SendAsync(new Update.Command(maintenanceEntryRequest));

            Assert.NotNull(maintenanceEntryResponse);

            var fromDb = await fixture.GetNisshiContext().MaintenanceEntries.FindAsync(maintenanceEntryResponse.Id);

            Assert.NotNull(fromDb);

            Assert.Equal(maintenanceEntryRequest.Comments, maintenanceEntryResponse.Comments);
            Assert.Equal(maintenanceEntryRequest.RepairPrice, maintenanceEntryResponse.RepairPrice);
            Assert.Equal(maintenanceEntryRequest.DatePerformed, maintenanceEntryResponse.DatePerformed);
            Assert.Equal(maintenanceEntryRequest.Type, maintenanceEntryResponse.Type);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Update.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_Doesnt_Exist()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testMaintenanceEntry = await Helpers.CreateTestMaintenanceEntry(fixture, user);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Update.Command(testMaintenanceEntry)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
