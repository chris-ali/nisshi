using System;
using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using Nisshi.Requests.LogbookEntries;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.LogbookEntries
{
    /// <summary>
    /// Tests editing a logbook entry in various scenarios
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
            var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);
            var logbookEntryRequest = await Helpers.SaveAndGet<LogbookEntry>(fixture, testLogbookEntry);

            logbookEntryRequest.Comments = "New test comments.";
            logbookEntryRequest.CrossCountry = 99.9m;
            logbookEntryRequest.FlightDate = DateTime.Today.AddMonths(12);
            logbookEntryRequest.NumLandings = 99;

            var logbookEntryResponse = await fixture.SendAsync(new Update.Command(logbookEntryRequest));

            Assert.NotNull(logbookEntryResponse);

            var fromDb = await fixture.GetNisshiContext().LogbookEntries.FindAsync(logbookEntryResponse.Id);

            Assert.NotNull(fromDb);

            Assert.Equal(logbookEntryRequest.Comments, logbookEntryResponse.Comments);
            Assert.Equal(logbookEntryRequest.CrossCountry, logbookEntryResponse.CrossCountry);
            Assert.Equal(logbookEntryRequest.FlightDate, logbookEntryResponse.FlightDate);
            Assert.Equal(logbookEntryRequest.NumLandings, logbookEntryResponse.NumLandings);
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
            var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Update.Command(testLogbookEntry)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
