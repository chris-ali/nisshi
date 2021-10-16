using System;
using System.Threading.Tasks;
using Nisshi.Models;
using Nisshi.Requests.Aircrafts;
using Xunit;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.IntegrationTests.Requests.Aircrafts
{
    /// <summary>
    /// Tests getting an aircraft in various scenarios
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
            var testAircraft = await Helpers.CreateTestAircraft(fixture, user);
            var aircraftRequest = await Helpers.SaveAndGet<Aircraft>(fixture, testAircraft);

            var aircraftResponse = await fixture.SendAsync(new GetOneById.Query(aircraftRequest.Id));

            Assert.NotNull(aircraftResponse);
            Assert.Equal(aircraftRequest.TailNumber, aircraftResponse.TailNumber);
            Assert.Equal(aircraftRequest.InstanceType, aircraftResponse.InstanceType);
            Assert.Equal(aircraftRequest.LastEngineHobbs, aircraftResponse.LastEngineHobbs);
            Assert.Equal(aircraftRequest.LastVOR, aircraftResponse.LastVOR);
        }

        [Fact]
        public async Task Should_Find_Three()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testAircraft = await Helpers.CreateTestAircraft(fixture, user);
            var testAircraft1 = await Helpers.CreateTestAircraft(fixture, user);
            var testAircraft2 = await Helpers.CreateTestAircraft(fixture, user);
            
            await fixture.GetNisshiContext().Aircraft.AddRangeAsync(new[] {testAircraft,testAircraft1,testAircraft2});
            await fixture.GetNisshiContext().SaveChangesAsync();
            
            var aircraftResponse = await fixture.SendAsync(new GetAll.Query());

            Assert.NotNull(aircraftResponse);
            Assert.Equal(3, aircraftResponse.Count);
        }

        [Fact]
        public async Task Should_Find_None()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var aircraftResponse = await fixture.SendAsync(new GetAll.Query());

            Assert.NotNull(aircraftResponse);
            Assert.Equal(0, aircraftResponse.Count);
        }

        [Fact]
        public async Task Should_Not_Find_Non_Existant_Aircraft()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var aircraftResponse = await fixture.SendAsync(new GetOneById.Query(11236));

            Assert.Null(aircraftResponse);
        }

        [Fact]
        public async Task Should_Not_Find_Other_Users_Aircraft()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var aircraftResponse = await fixture.SendAsync(new GetOneById.Query(1));

            Assert.Null(aircraftResponse);
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
