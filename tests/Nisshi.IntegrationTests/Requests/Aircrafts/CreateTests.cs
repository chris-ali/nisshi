using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Errors;
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
            var aircraftRequest = await Helpers.CreateTestAircraft(fixture, user);

            var aircraftResponse = await fixture.SendAsync(new Create.Command(aircraftRequest));

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
            
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Create.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_No_User()
        {
            var aircraftRequest = await Helpers.CreateTestAircraft(fixture, null);

            await Assert.ThrowsAsync<RestException>(() => fixture.SendAsync(new Create.Command(null)));
        } 
    }
}
