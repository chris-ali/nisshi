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
            var testAircraft = await Helpers.CreateTestAircraft(fixture, user);
            var aircraftRequest = await Helpers.SaveAndGet<Aircraft>(fixture, testAircraft);

            aircraftRequest.TailNumber = "N9440D";
            aircraftRequest.InstanceType = InstanceType.Invalid;
            aircraftRequest.LastVOR = DateTime.Today;
            aircraftRequest.LastEngineHobbs = 500;
            
            var aircraftResponse = await fixture.SendAsync(new Update.Command(aircraftRequest));

            Assert.NotNull(aircraftResponse);

            var fromDb = await fixture.GetNisshiContext().Aircraft.FindAsync(aircraftResponse.Id);
            
            Assert.NotNull(fromDb);

            Assert.Equal(fromDb.TailNumber, aircraftResponse.TailNumber);
            Assert.Equal(fromDb.InstanceType, aircraftResponse.InstanceType);
            Assert.Equal(fromDb.LastEngineHobbs, aircraftResponse.LastEngineHobbs);
            Assert.Equal(fromDb.LastVOR, aircraftResponse.LastVOR);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Update.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_No_User()
        {
            var aircraftRequest = await Helpers.CreateTestAircraft(fixture, null);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Update.Command(aircraftRequest)));
        }

        [Fact]
        public async Task Should_Fail_Doesnt_Exist()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testAircraft = await Helpers.CreateTestAircraft(fixture, user);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Update.Command(testAircraft)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
