using System;
using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using Nisshi.Requests.Aircrafts;
using Xunit;

/// <summary>
/// sic
/// </summary>
namespace Nisshi.IntegrationTests.Requests.Aircrafts
{
    /// <summary>
    /// Tests creating an aircraft in various scenarios
    /// </summary>
    public class GetTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture fixture;

        public GetTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Find_One()
        {
            var user = await Helpers.RegisterTestUser(fixture);
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
            var user = await Helpers.RegisterTestUser(fixture);
            var testAircraft = await Helpers.CreateTestAircraft(fixture, user);
            var testAircraft1 = await Helpers.CreateTestAircraft(fixture, user);
            var testAircraft2 = await Helpers.CreateTestAircraft(fixture, user);
            
            await fixture.GetNisshiContext().Aircraft.AddRangeAsync(new[] {testAircraft,testAircraft1,testAircraft2});
            await fixture.GetNisshiContext().SaveChangesAsync();
            
            var aircraftResponse = await fixture.SendAsync(new GetAll.Query());

            Assert.NotNull(aircraftResponse);
            Assert.Equal(aircraftResponse.Count, 3);
        }

        [Fact]
        public async Task Should_Find_None()
        {
            var aircraftResponse = await fixture.SendAsync(new GetAll.Query());

            Assert.NotNull(aircraftResponse);
            Assert.Equal(aircraftResponse.Count, 0);
        } 
    }
}
