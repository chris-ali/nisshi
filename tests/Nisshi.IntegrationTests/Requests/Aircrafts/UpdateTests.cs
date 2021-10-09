using System;
using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Enums;
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
    /// Tests editing an aircraft in various scenarios
    /// </summary>
    public class UpdateTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture fixture;

        public UpdateTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Have_Been_Updated()
        {
            var user = await Helpers.RegisterTestUser(fixture);
            var testAircraft = await Helpers.CreateTestAircraft(fixture, user);
            var aircraftRequest = await Helpers.SaveAndGet<Aircraft>(fixture, testAircraft);

            aircraftRequest.TailNumber = "N9440D";
            aircraftRequest.InstanceType = InstanceType.Invalid;
            aircraftRequest.LastVOR = DateTime.Today;
            aircraftRequest.LastEngineHobbs = 500;
            
            var aircraftResponse = await fixture.SendAsync(new Update.Command(aircraftRequest));

            Assert.NotNull(aircraftResponse);
            Assert.Equal(aircraftRequest.TailNumber, aircraftResponse.TailNumber);
            Assert.Equal(aircraftRequest.InstanceType, aircraftResponse.InstanceType);
            Assert.Equal(aircraftRequest.LastEngineHobbs, aircraftResponse.LastEngineHobbs);
            Assert.Equal(aircraftRequest.LastVOR, aircraftResponse.LastVOR);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            var user = await Helpers.RegisterTestUser(fixture);
            
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Update.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_No_User()
        {
            var aircraftRequest = await Helpers.CreateTestAircraft(fixture, null);

            await Assert.ThrowsAsync<RestException>(() => fixture.SendAsync(new Update.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_Doesnt_Exist()
        {
            var user = await Helpers.RegisterTestUser(fixture);
            var testAircraft = await Helpers.CreateTestAircraft(fixture, user);

            await Assert.ThrowsAsync<RestException>(() => fixture.SendAsync(new Update.Command(testAircraft)));
        }
    }
}
