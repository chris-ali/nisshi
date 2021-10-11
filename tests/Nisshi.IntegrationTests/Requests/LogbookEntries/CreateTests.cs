using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Errors;
using Nisshi.Requests.LogbookEntries;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.LogbookEntries
{
    /// <summary>
    /// Tests creating a logbook entry in various scenarios
    /// </summary>
    public class CreateTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture fixture;

        public CreateTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Have_Been_Created()
        {
            var user = await Helpers.RegisterTestUser(fixture);
            var logbookEntryRequest = await Helpers.CreateTestLogbookEntry(fixture, user);

            var logbookEntryResponse = await fixture.SendAsync(new Create.Command(logbookEntryRequest));

            Assert.NotNull(logbookEntryResponse);
            Assert.Equal(logbookEntryRequest.Comments, logbookEntryResponse.Comments);
            Assert.Equal(logbookEntryRequest.CrossCountry, logbookEntryResponse.CrossCountry);
            Assert.Equal(logbookEntryRequest.FlightDate, logbookEntryResponse.FlightDate);
            Assert.Equal(logbookEntryRequest.NumLandings, logbookEntryResponse.NumLandings);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            var user = await Helpers.RegisterTestUser(fixture);
            
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Create.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_No_User()
        {
            var logbookEntryRequest = await Helpers.CreateTestLogbookEntry(fixture, null);

            await Assert.ThrowsAsync<RestException>(() => fixture.SendAsync(new Create.Command(logbookEntryRequest)));
        } 
    }
}
