using System;
using Nisshi.Models;
using Xunit;

namespace Nisshi.UnitTests.Aircrafts
{
    /// <summary>
    /// Tests aircraft validation in various scenarios
    /// </summary>
    public class AircraftTests
    {
        [Fact]
        public void Should_Pass_Validation()
        {
            var decTime = 100.1m;
            var dateAdjuster = -1;
            var tailNumber = "N9440D";
            var aircraft = GenerateAircraft(decTime, dateAdjuster, tailNumber);

            var errors = new Aircraft.AircraftValidator().Validate(aircraft);
            Assert.Empty(errors.Errors);

            aircraft.LastTransponder = aircraft.LastAltimeter = aircraft.LastAnnual =
                aircraft.LastELT = aircraft.LastPitotStatic = aircraft.LastVOR =
                aircraft.RegistrationDue = null;

            errors = new Aircraft.AircraftValidator().Validate(aircraft);
            Assert.Empty(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Too_Long_Empty()
        {
            var decTime = 100.1m;
            var dateAdjuster = -1;
            var tailNumber = "N9440DN9440DN9440DN9440DN9440DN9440DN9440D";
            var aircraft = GenerateAircraft(decTime, dateAdjuster, tailNumber);

            var errors = new Aircraft.AircraftValidator().Validate(aircraft);
            Assert.Single(errors.Errors);

            aircraft.TailNumber = "";

            errors = new Aircraft.AircraftValidator().Validate(aircraft);
            Assert.Single(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Bad_Numerics()
        {
            var decTime = -100.1m;
            var dateAdjuster = 1;
            var tailNumber = "N9440D";
            var aircraft = GenerateAircraft(decTime, dateAdjuster, tailNumber);

            var errors = new Aircraft.AircraftValidator().Validate(aircraft);
            Assert.Equal(10, errors.Errors.Count);
        }

        private static Aircraft GenerateAircraft(decimal decTime, int dateAdjuster, string tailNumber) => new Aircraft
        {
            TailNumber = tailNumber,
            Last100Hobbs = decTime,
            LastEngineHobbs = decTime,
            LastOilHobbs = decTime,
            LastAltimeter = DateTime.Now.AddMonths(1 * dateAdjuster),
            LastAnnual = DateTime.Now.AddMonths(1 * dateAdjuster),
            LastELT = DateTime.Now.AddMonths(1 * dateAdjuster),
            LastPitotStatic = DateTime.Now.AddMonths(1 * dateAdjuster),
            LastTransponder = DateTime.Now.AddMonths(1 * dateAdjuster),
            LastVOR = DateTime.Now.AddMonths(1 * dateAdjuster),
            RegistrationDue = DateTime.Now.AddMonths(-1 * dateAdjuster),
        };
    }
}
