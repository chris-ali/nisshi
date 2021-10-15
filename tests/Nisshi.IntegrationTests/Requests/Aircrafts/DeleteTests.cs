using System;
using System.Threading.Tasks;
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
    /// Tests deleting an aircraft in various scenarios
    /// </summary>
    public class DeleteTests : IClassFixture<SliceFixture>, IDisposable
    {
        private readonly SliceFixture fixture;

        public DeleteTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Have_Been_Deleted()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testAircraft = await Helpers.CreateTestAircraft(fixture, user);
            var aircraftRequest = await Helpers.SaveAndGet<Aircraft>(fixture, testAircraft);
            
            var aircraftResponse = await fixture.SendAsync(new Delete.Command(aircraftRequest.Id));

            Assert.NotNull(aircraftResponse);
            
            aircraftResponse = await fixture.SendAsync(new GetOneById.Query(aircraftRequest.Id));

            Assert.Null(aircraftResponse);
        }

        [Fact]
        public async Task Should_Fail_Doesnt_Exist()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);
            var testAircraft = await Helpers.CreateTestAircraft(fixture, user);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Delete.Command(testAircraft.Id)));
        }

        [Fact]
        public async Task Should_Not_Delete_Other_Users_Aircraft()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Delete.Command(1)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
