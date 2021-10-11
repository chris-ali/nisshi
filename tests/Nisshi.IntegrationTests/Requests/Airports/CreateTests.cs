using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using Nisshi.Requests.Airports;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Airports
{
    /// <summary>
    /// Tests creating an airport in various scenarios
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
            var airportRequest = new Airport
            {
                AirportCode = "KDAB",
                FacilityName = "Daytona Beach International",
                Latitude = 31.01,
                Longitude = -70.2,
                Preferred = true,
                Type = "A",
                SourceUserName = Helpers.TestUserName
            };

            var airportResponse = await fixture.SendAsync(new Create.Command(airportRequest));

            Assert.NotNull(airportResponse);
            Assert.Equal(airportRequest.AirportCode, airportResponse.AirportCode);
            Assert.Equal(airportRequest.Latitude, airportResponse.Latitude);
            Assert.Equal(airportRequest.Preferred, airportResponse.Preferred);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Create.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_Already_Exists()
        {
            var airportRequest = new Airport
            {
                AirportCode = "KTTN",
                FacilityName = "Trenton Mercer Airport",
                Latitude = 41.01,
                Longitude = -70.2,
                Preferred = true,
                Type = "A",
                SourceUserName = Helpers.TestUserName
            };

            await Assert.ThrowsAsync<RestException>(() => fixture.SendAsync(new Create.Command(airportRequest)));
        }
    }
}
