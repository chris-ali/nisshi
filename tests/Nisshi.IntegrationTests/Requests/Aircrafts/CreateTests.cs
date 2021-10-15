using System;
using System.Threading.Tasks;
using FluentValidation;
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
    public class CreateTests : IClassFixture<SliceFixture>, IDisposable
    {
        private readonly SliceFixture fixture;

        public CreateTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Have_Been_Created()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var aircraftRequest = await Helpers.CreateTestAircraft(fixture, user);

            var aircraftResponse = await fixture.SendAsync(new Create.Command(aircraftRequest));

            Assert.NotNull(aircraftResponse);

            var fromDb = await fixture.GetNisshiContext().Aircraft.FindAsync(aircraftResponse.Id);
            
            Assert.NotNull(fromDb);

            Assert.Equal(fromDb.TailNumber, aircraftResponse.TailNumber);
            Assert.Equal(fromDb.InstanceType, aircraftResponse.InstanceType);
            Assert.Equal(fromDb.LastEngineHobbs, aircraftResponse.LastEngineHobbs);
            Assert.Equal(fromDb.LastVOR, aircraftResponse.LastVOR);
            Assert.Equal(fromDb.Last100Hobbs, aircraftResponse.Last100Hobbs);
            Assert.Equal(fromDb.LastAltimeter, aircraftResponse.LastAltimeter);
            Assert.Equal(fromDb.LastELT, aircraftResponse.LastELT);
            Assert.Equal(fromDb.LastOilHobbs, aircraftResponse.LastOilHobbs);
            Assert.Equal(fromDb.LastPitotStatic, aircraftResponse.LastPitotStatic);
            Assert.Equal(fromDb.LastTransponder, aircraftResponse.LastTransponder);
            Assert.Equal(fromDb.Notes, aircraftResponse.Notes);
            Assert.Equal(fromDb.RegistrationDue, aircraftResponse.RegistrationDue);
            Assert.Equal(fromDb.TailNumber, aircraftResponse.TailNumber);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Create.Command(null)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
