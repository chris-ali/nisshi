using System;
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
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Delete.Command(testLogbookEntry.Id)));
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
