using Nisshi.Models;
using Xunit;

namespace Nisshi.UnitTests.Models
{
    /// <summary>
    /// Tests manufacturer validation in various scenarios
    /// </summary>
    public class ManufacturerTests
    {
        [Fact]
        public void Should_Pass_Validation()
        {
            var manufacturer = new Manufacturer
            {
                ManufacturerName = "Cessna"
            };

            var errors = new Manufacturer.ManufacturerValidator().Validate(manufacturer);
            Assert.Empty(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Too_Long_Empty()
        {
            var manufacturer = new Manufacturer
            {
                ManufacturerName = "Cessna Cessna Cessna Cessna Cessna Cessna Cessna Cessna Cessna "
            };

            var errors = new Manufacturer.ManufacturerValidator().Validate(manufacturer);
            Assert.Single(errors.Errors);

            manufacturer.ManufacturerName = "";

            errors = new Manufacturer.ManufacturerValidator().Validate(manufacturer);
            Assert.Single(errors.Errors);
        }
    }
}
