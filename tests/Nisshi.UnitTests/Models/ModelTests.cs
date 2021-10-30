using Nisshi.Models;
using Xunit;

namespace Nisshi.UnitTests.Models
{
    /// <summary>
    /// Tests model validation in various scenarios
    /// </summary>
    public class ModelTests
    {
        [Fact]
        public void Should_Pass_Validation()
        {
            var model = new Model
            {
                ModelName = "172N",
                Family = "172",
                TypeName = "172"
            };

            var errors = new Model.ModelValidator().Validate(model);
            Assert.Empty(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Too_Long_Empty()
        {
            var model = new Model
            {
                ModelName = "172N172N172N172N172N172N172N172N172N172N172N172N172N172N172N172N172N",
                Family = "172172172172172172172172172172172172172172172172172172172172172172172",
                TypeName = "172172172172172172172172172172172172172172172172172172172172172172172"
            };

            var errors = new Model.ModelValidator().Validate(model);
            Assert.Equal(3, errors.Errors.Count);

            model.ModelName = "";

            errors = new Model.ModelValidator().Validate(model);
            Assert.Equal(3, errors.Errors.Count);
        }
    }
}
