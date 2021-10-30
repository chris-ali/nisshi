using Nisshi.Models;
using Xunit;

namespace Nisshi.UnitTests.Models
{
    /// <summary>
    /// Tests airport validation in various scenarios
    /// </summary>
    public class AirportTests
    {
        [Fact]
        public void Should_Pass_Validation()
        {
            var airport = new Airport
            {
                AirportCode = "KTTN",
                FacilityName = "Trenton Mercer Airport",
                Latitude = 42.1,
                Longitude = -74.8
            };

            var errors = new Airport.AirportValidator().Validate(airport);
            Assert.Empty(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Too_Long_Empty()
        {
            var airport = new Airport
            {
                AirportCode = "KTTNKTTNKTTNKTTNKTTNKTTN",
                FacilityName = "Trenton Mercer Airport Trenton Mercer Airport Trenton Mercer Airport Trenton Mercer Airport Trenton Mercer Airport Trenton Mercer Airport Trenton Mercer Airport",
                Latitude = 42.1,
                Longitude = -74.8
            };

            var errors = new Airport.AirportValidator().Validate(airport);
            Assert.Equal(2, errors.Errors.Count);

            airport.AirportCode = "";
            airport.FacilityName = "";

            errors = new Airport.AirportValidator().Validate(airport);
            Assert.Equal(3, errors.Errors.Count);
        }

        [Fact]
        public void Should_Fail_Validation_Bad_Numerics()
        {
            var airport = new Airport
            {
                AirportCode = "KTTN",
                FacilityName = "Trenton Mercer Airport",
                Latitude = 91,
                Longitude = -181
            };

            var errors = new Airport.AirportValidator().Validate(airport);
            Assert.Equal(2, errors.Errors.Count);

            airport.Latitude = -91;
            airport.Longitude = -181;

            errors = new Airport.AirportValidator().Validate(airport);
            Assert.Equal(2, errors.Errors.Count);
        }
    }
}
