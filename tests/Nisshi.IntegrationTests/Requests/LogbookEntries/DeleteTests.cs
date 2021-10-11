using System.Threading.Tasks;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using Nisshi.Requests.LogbookEntries;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.LogbookEntries
{
    /// <summary>
    /// Tests deleting a logbook entry in various scenarios
    /// </summary>
    public class DeleteTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture fixture;

        public DeleteTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Have_Been_Deleted()
        {
            var user = await Helpers.RegisterTestUser(fixture);
            var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);
            var logbookEntryRequest = await Helpers.SaveAndGet<LogbookEntry>(fixture, testLogbookEntry);
            
            var logbookEntryResponse = await fixture.SendAsync(new Delete.Command(logbookEntryRequest.Id));

            Assert.NotNull(logbookEntryResponse);
            
            logbookEntryResponse = await fixture.SendAsync(new GetOneById.Query(logbookEntryRequest.Id));

            Assert.Null(logbookEntryResponse);
        }

        [Fact]
        public async Task Should_Fail_Doesnt_Exist()
        {
            var user = await Helpers.RegisterTestUser(fixture);
            var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);

            await Assert.ThrowsAsync<RestException>(() => fixture.SendAsync(new Delete.Command(testLogbookEntry.Id)));
        }
    }
}
