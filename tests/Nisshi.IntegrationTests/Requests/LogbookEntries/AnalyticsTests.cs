using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task Should_Sum_Total_Time_By_Month()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            for (int i = 0; i < 200; i++)
            {
                var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);
                await fixture.GetNisshiContext().LogbookEntries.AddAsync(testLogbookEntry);
            }

            await fixture.GetNisshiContext().SaveChangesAsync();

            var analyticsResponse = await fixture.SendAsync(new GetSumTotalTimeGroupByMonth.Query());
            Assert.NotNull(analyticsResponse);
            Assert.True(analyticsResponse.Count > 0);

            var logbookEntryResponse = await fixture.SendAsync(new GetAll.Query());
            Assert.NotNull(logbookEntryResponse);
            Assert.True(logbookEntryResponse.Count > 0);

            Assert.Equal(logbookEntryResponse.Sum(x => x.TotalFlightTime),
                analyticsResponse.Sum(x => x.TotalTimeSum));
        }

        [Fact]
        public async Task Should_Sum_Total_Time_By_CatClass()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            for (int i = 0; i < 200; i++)
            {
                var testLogbookEntry = await Helpers.CreateTestLogbookEntry(fixture, user);
                testLogbookEntry.Aircraft = fixture.GetNisshiContext().Aircraft.ToList()[i%3];
                await fixture.GetNisshiContext().LogbookEntries.AddAsync(testLogbookEntry);
            }

            await fixture.GetNisshiContext().SaveChangesAsync();

            var analyticsResponse = await fixture.SendAsync(new GetSumTotalTimeGroupByCatClass.Query());
            Assert.NotNull(analyticsResponse);
            Assert.True(analyticsResponse.Count > 0);

            var logbookEntryResponse = await fixture.SendAsync(new GetAll.Query());
            Assert.NotNull(logbookEntryResponse);
            Assert.True(logbookEntryResponse.Count > 0);

            Assert.Equal(logbookEntryResponse.Sum(x => x.TotalFlightTime),
                analyticsResponse.Sum(x => x.TotalTimeSum));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
