using System;
using System.Threading.Tasks;
using Nisshi.Models;
using Nisshi.Requests.LogbookEntries;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.LogbookEntries
{
    /// <summary>
    /// Tests getting a logbook entry in various scenarios
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
            var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);
            var logbookEntryRequest = await Helpers.SaveAndGet<LogbookEntry>(fixture, testLogbookEntry);

            var logbookEntryResponse = await fixture.SendAsync(new GetOneById.Query(logbookEntryRequest.Id));

            Assert.NotNull(logbookEntryResponse);
            Assert.Equal(logbookEntryRequest.Comments, logbookEntryResponse.Comments);
            Assert.Equal(logbookEntryRequest.CrossCountry, logbookEntryResponse.CrossCountry);
            Assert.Equal(logbookEntryRequest.FlightDate, logbookEntryResponse.FlightDate);
            Assert.Equal(logbookEntryRequest.NumLandings, logbookEntryResponse.NumLandings);
        }

        [Fact]
        public async Task Should_Find_Three()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);
            var testLogbookEntry1 = await Helpers.CreateTestLogbookEntry(fixture, user);
            var testLogbookEntry2 = await Helpers.CreateTestLogbookEntry(fixture, user);
            
            await fixture.GetNisshiContext().LogbookEntries.AddRangeAsync(new[] {testLogbookEntry,testLogbookEntry1,testLogbookEntry2});
            await fixture.GetNisshiContext().SaveChangesAsync();
            
            var logbookEntryResponse = await fixture.SendAsync(new GetAll.Query());

            Assert.NotNull(logbookEntryResponse);
            Assert.Equal(3, logbookEntryResponse.Count);
        }

        [Fact]
        public async Task Should_Find_None()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var logbookEntryResponse = await fixture.SendAsync(new GetAll.Query());

            Assert.NotNull(logbookEntryResponse);
            Assert.Equal(0, logbookEntryResponse.Count);
        }

        [Fact]
        public async Task Should_Not_Find_Non_Existant_Entry()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var logbookEntryResponse = await fixture.SendAsync(new GetOneById.Query(11236));

            Assert.Null(logbookEntryResponse);
        }

        [Fact]
        public async Task Should_Not_Find_Other_Users_Entry()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var logbookEntryResponse = await fixture.SendAsync(new GetOneById.Query(1));

            Assert.Null(logbookEntryResponse);
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
