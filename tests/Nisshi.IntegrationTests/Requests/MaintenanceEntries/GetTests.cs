using System;
using System.Threading.Tasks;
using Nisshi.Models;
using Nisshi.Requests.MaintenanceEntries;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.MaintenanceEntries
{
    /// <summary>
    /// Tests getting a maintenance entry in various scenarios
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
            var testMaintenanceEntry = await Helpers.CreateTestMaintenanceEntry(fixture, user);
            var maintenanceEntryRequest = await Helpers.SaveAndGet<MaintenanceEntry>(fixture, testMaintenanceEntry);

            var maintenanceEntryResponse = await fixture.SendAsync(new GetOneById.Query(maintenanceEntryRequest.Id));

            Assert.NotNull(maintenanceEntryResponse);
            Assert.Equal(maintenanceEntryRequest.Comments, maintenanceEntryResponse.Comments);
            Assert.Equal(maintenanceEntryRequest.DatePerformed, maintenanceEntryResponse.DatePerformed);
            Assert.Equal(maintenanceEntryRequest.RepairPrice, maintenanceEntryResponse.RepairPrice);
            Assert.Equal(maintenanceEntryRequest.Type, maintenanceEntryResponse.Type);
        }

        [Fact]
        public async Task Should_Find_Three()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testMaintenanceEntry = await Helpers.CreateTestMaintenanceEntry(fixture, user);
            var testMaintenanceEntry1 = await Helpers.CreateTestMaintenanceEntry(fixture, user);
            var testMaintenanceEntry2 = await Helpers.CreateTestMaintenanceEntry(fixture, user);

            await fixture.GetNisshiContext().MaintenanceEntries.AddRangeAsync(new[] { testMaintenanceEntry, testMaintenanceEntry1, testMaintenanceEntry2 });
            await fixture.GetNisshiContext().SaveChangesAsync();

            var maintenanceEntryResponse = await fixture.SendAsync(new GetAll.Query());

            Assert.NotNull(maintenanceEntryResponse);
            Assert.Equal(3, maintenanceEntryResponse.Count);
        }

        [Fact]
        public async Task Should_Find_None()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var maintenanceEntryResponse = await fixture.SendAsync(new GetAll.Query());

            Assert.NotNull(maintenanceEntryResponse);
            Assert.Equal(0, maintenanceEntryResponse.Count);
        }

        [Fact]
        public async Task Should_Not_Find_Non_Existant_Entry()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var maintenanceEntryResponse = await fixture.SendAsync(new GetOneById.Query(11236));

            Assert.Null(maintenanceEntryResponse);
        }

        [Fact]
        public async Task Should_Not_Find_Other_Users_Entry()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var maintenanceEntryResponse = await fixture.SendAsync(new GetOneById.Query(1));

            Assert.Null(maintenanceEntryResponse);
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
