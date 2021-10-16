using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using Nisshi.Requests.Airports;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Airports
{
    /// <summary>
    /// Tests creating an airport in various scenarios
    /// </summary>
    public class CreateTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public CreateTests()
        {
            this.fixture = new SliceFixture();
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

            var fromDb = await fixture.GetNisshiContext().Airports
                .FirstOrDefaultAsync(x => x.AirportCode == airportResponse.AirportCode);
            
            Assert.NotNull(fromDb);

            Assert.Equal(fromDb.AirportCode, airportResponse.AirportCode);
            Assert.Equal(fromDb.Latitude, airportResponse.Latitude);
            Assert.Equal(fromDb.Longitude, airportResponse.Longitude);
            Assert.Equal(fromDb.Preferred, airportResponse.Preferred);
            Assert.Equal(fromDb.Type, airportResponse.Type);
            Assert.Equal(fromDb.SourceUserName, airportResponse.SourceUserName);
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

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new Create.Command(airportRequest)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
