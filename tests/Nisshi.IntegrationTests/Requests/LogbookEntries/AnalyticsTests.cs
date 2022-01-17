using System;
using System.Linq;
using System.Threading.Tasks;
using Nisshi.Infrastructure.Enums;
using Nisshi.Requests.LogbookEntries;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.LogbookEntries
{
    /// <summary>
    /// Tests getting various types of logbook entry analytics
    /// </summary>
    public class AnalyticsTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public AnalyticsTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Should_Sum_Totals_By_Month()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            for (int i = 0; i < 200; i++)
            {
                var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);
                await fixture.GetNisshiContext().LogbookEntries.AddAsync(testLogbookEntry);
            }

            await fixture.GetNisshiContext().SaveChangesAsync();

            var analyticsResponse = await fixture.SendAsync(new GetSumTotalsGroupByMonth.Query());
            Assert.NotNull(analyticsResponse);
            Assert.True(analyticsResponse.Count > 0);

            var logbookEntryResponse = await fixture.SendAsync(new GetAll.Query());
            Assert.NotNull(logbookEntryResponse);
            Assert.True(logbookEntryResponse.Count > 0);

            Assert.Equal(logbookEntryResponse.Sum(x => x.TotalFlightTime),
                analyticsResponse.Sum(x => x.TotalTimeSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.IMC),
                analyticsResponse.Sum(x => x.InstrumentSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.Turbine),
                analyticsResponse.Sum(x => x.TurbineSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.Night),
                analyticsResponse.Sum(x => x.NightSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.CrossCountry),
                analyticsResponse.Sum(x => x.CrossCountrySum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.PIC),
                analyticsResponse.Sum(x => x.PICSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.SIC),
                analyticsResponse.Sum(x => x.SICSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.DualGiven),
                analyticsResponse.Sum(x => x.DualGivenSum));
        }

        [Fact]
        public async Task Should_Sum_Totals_By_CatClass()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            for (int i = 0; i < 200; i++)
            {
                var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);
                testLogbookEntry.Aircraft = fixture.GetNisshiContext().Aircraft.ToList()[i%3];
                await fixture.GetNisshiContext().LogbookEntries.AddAsync(testLogbookEntry);
            }

            await fixture.GetNisshiContext().SaveChangesAsync();

            var analyticsResponse = await fixture.SendAsync(new GetSumTotalsGroupByCatClass.Query());
            Assert.NotNull(analyticsResponse);
            Assert.True(analyticsResponse.Count > 0);

            var logbookEntryResponse = await fixture.SendAsync(new GetAll.Query());
            Assert.NotNull(logbookEntryResponse);
            Assert.True(logbookEntryResponse.Count > 0);

            Assert.Equal(logbookEntryResponse.Sum(x => x.TotalFlightTime),
                analyticsResponse.Sum(x => x.TotalTimeSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.IMC),
                analyticsResponse.Sum(x => x.InstrumentSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.Turbine),
                analyticsResponse.Sum(x => x.TurbineSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.Night),
                analyticsResponse.Sum(x => x.NightSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.CrossCountry),
                analyticsResponse.Sum(x => x.CrossCountrySum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.PIC),
                analyticsResponse.Sum(x => x.PICSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.SIC),
                analyticsResponse.Sum(x => x.SICSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.DualGiven),
                analyticsResponse.Sum(x => x.DualGivenSum));
        }

        [Fact]
        public async Task Should_Sum_Totals_By_Type()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            for (int i = 0; i < 200; i++)
            {
                var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);
                testLogbookEntry.Aircraft = fixture.GetNisshiContext().Aircraft.ToList()[i%3];
                await fixture.GetNisshiContext().LogbookEntries.AddAsync(testLogbookEntry);
            }

            await fixture.GetNisshiContext().SaveChangesAsync();

            var analyticsResponse = await fixture.SendAsync(new GetSumTotalsGroupByType.Query());
            Assert.NotNull(analyticsResponse);
            Assert.True(analyticsResponse.Count > 0);

            var logbookEntryResponse = await fixture.SendAsync(new GetAll.Query());
            Assert.NotNull(logbookEntryResponse);
            Assert.True(logbookEntryResponse.Count > 0);

            Assert.Equal(logbookEntryResponse.Sum(x => x.TotalFlightTime),
                analyticsResponse.Sum(x => x.TotalTimeSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.IMC),
                analyticsResponse.Sum(x => x.InstrumentSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.Turbine),
                analyticsResponse.Sum(x => x.TurbineSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.Night),
                analyticsResponse.Sum(x => x.NightSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.CrossCountry),
                analyticsResponse.Sum(x => x.CrossCountrySum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.PIC),
                analyticsResponse.Sum(x => x.PICSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.SIC),
                analyticsResponse.Sum(x => x.SICSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.DualGiven),
                analyticsResponse.Sum(x => x.DualGivenSum));
        }

        [Fact]
        public async Task Should_Sum_Totals_By_Instance()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            for (int i = 0; i < 200; i++)
            {
                var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);
                testLogbookEntry.Aircraft = fixture.GetNisshiContext().Aircraft.ToList()[i%3];
                testLogbookEntry.Aircraft.InstanceType = (InstanceType)(i%3);
                await fixture.GetNisshiContext().LogbookEntries.AddAsync(testLogbookEntry);
            }

            await fixture.GetNisshiContext().SaveChangesAsync();

            var analyticsResponse = await fixture.SendAsync(new GetSumTotalsGroupByInstance.Query());
            Assert.NotNull(analyticsResponse);
            Assert.True(analyticsResponse.Count > 0);

            var logbookEntryResponse = await fixture.SendAsync(new GetAll.Query());
            Assert.NotNull(logbookEntryResponse);
            Assert.True(logbookEntryResponse.Count > 0);

            Assert.Equal(logbookEntryResponse.Sum(x => x.TotalFlightTime),
                analyticsResponse.Sum(x => x.TotalTimeSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.IMC),
                analyticsResponse.Sum(x => x.InstrumentSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.Turbine),
                analyticsResponse.Sum(x => x.TurbineSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.Night),
                analyticsResponse.Sum(x => x.NightSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.CrossCountry),
                analyticsResponse.Sum(x => x.CrossCountrySum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.PIC),
                analyticsResponse.Sum(x => x.PICSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.SIC),
                analyticsResponse.Sum(x => x.SICSum));

            Assert.Equal(logbookEntryResponse.Sum(x => x.DualGiven),
                analyticsResponse.Sum(x => x.DualGivenSum));
        }

        [Fact]
        public async Task Should_Sum_Landings_Approaches_In_Last_90_Days()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            for (int i = 0; i < 200; i++)
            {
                var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);
                await fixture.GetNisshiContext().LogbookEntries.AddAsync(testLogbookEntry);
            }

            await fixture.GetNisshiContext().SaveChangesAsync();

            var analyticsResponse = await fixture.SendAsync(new GetTotalLandingsApproachesPast90Days.Query());
            Assert.NotNull(analyticsResponse);

            var logbookEntryResponse = await fixture.SendAsync(new GetAll.Query());
            Assert.NotNull(logbookEntryResponse);
            Assert.True(logbookEntryResponse.Count > 0);

            var last90Days = logbookEntryResponse.Where(x => x.FlightDate >= DateTime.Today.AddDays(-90));

            Assert.Equal(last90Days.Sum(x => x.NumLandings),
                analyticsResponse.DayLandings);

            Assert.Equal(last90Days.Sum(x => x.NumNightLandings),
                analyticsResponse.NightLandings);

            Assert.Equal(last90Days.Sum(x => x.NumInstrumentApproaches),
                analyticsResponse.Approaches);
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
