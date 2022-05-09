using System;
using System.Threading.Tasks;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using Nisshi.Requests.MaintenanceEntries;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.MaintenanceEntries
{
    /// <summary>
    /// Tests deleting a maintenance entry in various scenarios
    /// </summary>
    public class DeleteTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public DeleteTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Should_Have_Been_Deleted()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testMaintenanceEntry = await Helpers.CreateTestMaintenanceEntry(fixture, user);
            var maintenanceEntryRequest = await Helpers.SaveAndGet<MaintenanceEntry>(fixture, testMaintenanceEntry);

            var maintenanceEntryResponse = await fixture.SendAsync(new Delete.Command(maintenanceEntryRequest.Id));

            Assert.NotNull(maintenanceEntryResponse);

            maintenanceEntryResponse = await fixture.SendAsync(new GetOneById.Query(maintenanceEntryRequest.Id));

            Assert.Null(maintenanceEntryResponse);
        }

        [Fact]
        public async Task Should_Fail_Doesnt_Exist()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testMaintenanceEntry = await Helpers.CreateTestMaintenanceEntry(fixture, user);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Delete.Command(testMaintenanceEntry.Id)));
        }

        [Fact]
        public async Task Should_Not_Delete_Other_Users_Aircraft()
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
