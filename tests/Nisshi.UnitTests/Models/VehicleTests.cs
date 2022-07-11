using System;
using Nisshi.Models;
using Xunit;

namespace Nisshi.UnitTests.Vehicles
{
    /// <summary>
    /// Tests vehicle validation in various scenarios
    /// </summary>
    public class VehicleTests
    {
        [Fact]
        public void Should_Pass_Validation()
        {
            var vehicle = new Vehicle
            {
                InspectionDue = DateTime.Today,
                LastInspection = DateTime.Today,
                RegistrationDue = DateTime.Today,
                LastRegistration = DateTime.Today,
                Make = "BMW",
                Model = "323i",
                Trim = "Base",
                Year = 2000,
                Miles = 104000,
                Vin = "WBAAB123456789123",
                Notes = "Test notes",
            };

            var errors = new Vehicle.VehicleValidator().Validate(vehicle);
            Assert.Empty(errors.Errors);

            vehicle.InspectionDue = vehicle.LastInspection = vehicle.RegistrationDue =
                vehicle.LastRegistration = null;

            errors = new Vehicle.VehicleValidator().Validate(vehicle);
            Assert.Empty(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Too_Long_Bad_Numerics()
        {
            var vehicle = new Vehicle
            {
                LastInspection = DateTime.Today.AddDays(10),
                LastRegistration = DateTime.Today.AddDays(10),
                Make = "BMWBMWBMWBMWBMWBMWBMWBMWBMWBMWBMWBMWBMWBMWBMWBMWBMWBMWBMW",
                Model = "323i323i323i323i323i323i323i323i323i323i323i323i323i323i",
                Trim = "BaseBaseBaseBaseBaseBaseBaseBaseBaseBaseBaseBaseBaseBaseBase",
                Year = -2000,
                Miles = -104000,
                Vin = "WBAAB123456789123WBAAB123456789123",
                Notes = @"Test notes Test notes Test notes Test notes
                Test notes Test notes Test notes Test notes Test notes
                Test notes Test notes Test notes Test notes Test notes
                Test notes Test notes Test notes Test notes Test notes
                Test notes Test notes Test notes Test notes Test notes
                Test notes Test notes",
            };

            var errors = new Vehicle.VehicleValidator().Validate(vehicle);
            Assert.Equal(9, errors.Errors.Count);
        }
    }
}
