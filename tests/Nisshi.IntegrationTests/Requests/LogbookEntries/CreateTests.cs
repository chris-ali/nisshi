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

            var fromDb = await fixture.GetNisshiContext().LogbookEntries.FindAsync(logbookEntryResponse.Id);
            
            Assert.NotNull(fromDb);

            Assert.Equal(fromDb.Comments, logbookEntryResponse.Comments);
            Assert.Equal(fromDb.CrossCountry, logbookEntryResponse.CrossCountry);
            Assert.Equal(fromDb.FlightDate, logbookEntryResponse.FlightDate);
            Assert.Equal(fromDb.NumLandings, logbookEntryResponse.NumLandings);
            Assert.Equal(fromDb.DualGiven, logbookEntryResponse.DualGiven);
            Assert.Equal(fromDb.DualReceived, logbookEntryResponse.DualReceived);
            Assert.Equal(fromDb.GroundSim, logbookEntryResponse.GroundSim);
            Assert.Equal(fromDb.HobbsEnd, logbookEntryResponse.HobbsEnd);
            Assert.Equal(fromDb.HobbsStart, logbookEntryResponse.HobbsStart);
            Assert.Equal(fromDb.IMC, logbookEntryResponse.IMC);
            Assert.Equal(fromDb.MultiEngine, logbookEntryResponse.MultiEngine);
            Assert.Equal(fromDb.Night, logbookEntryResponse.Night);
            Assert.Equal(fromDb.NumFullStopLandings, logbookEntryResponse.NumFullStopLandings);
            Assert.Equal(fromDb.NumInstrumentApproaches, logbookEntryResponse.NumInstrumentApproaches);
            Assert.Equal(fromDb.NumNightLandings, logbookEntryResponse.NumNightLandings);
            Assert.Equal(fromDb.PIC, logbookEntryResponse.PIC);
            Assert.Equal(fromDb.MultiEngine, logbookEntryResponse.MultiEngine);
            Assert.Equal(fromDb.SimulatedInstrument, logbookEntryResponse.SimulatedInstrument);
            Assert.Equal(fromDb.TotalFlightTime, logbookEntryResponse.TotalFlightTime);
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
