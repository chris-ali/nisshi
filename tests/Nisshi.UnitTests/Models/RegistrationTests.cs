using Nisshi.Models.Users;
using Xunit;

namespace Nisshi.UnitTests.Models
{
    /// <summary>
    /// Tests registration validation in various scenarios
    /// </summary>
    public class RegistrationTests
    {
        [Fact]
        public void Should_Pass_Validation()
        {
            var register = new Registration
            {
                Username = "chris",
                Password = "aPas$tesT123!",
                Email = "chris@test.com"
            };

            var errors = new Registration.RegistrationValidator().Validate(register);
            Assert.Empty(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Too_Long_Too_Short_Empty()
        {
            var register = new Registration
            {
                Username = "christophechristophechristophechristophechristophechristophechristophe",
                Password = "tesT123!tesT123!tesT123!",
                Email = "christophechristophechristophechristophechristophechristophechristophe@test.com"
            };

            var errors = new Registration.RegistrationValidator().Validate(register);
            Assert.Equal(3, errors.Errors.Count);

            register.Username = "";
            register.Email = "";
            register.Password = "Tes!1";

            errors = new Registration.RegistrationValidator().Validate(register);
            Assert.Equal(3, errors.Errors.Count);

            register.Password = "";

            errors = new Registration.RegistrationValidator().Validate(register);
            Assert.Equal(3, errors.Errors.Count);
        }

        [Fact]
        public void Should_Fail_Validation_Bad_Password()
        {
            var register = new Registration
            {
                Username = "chris",
                Password = "test123!",
                Email = "chris@test.com"
            };

            var errors = new Registration.RegistrationValidator().Validate(register);
            Assert.Single(errors.Errors);

            register.Password = "test1234";

            errors = new Registration.RegistrationValidator().Validate(register);
            Assert.Single(errors.Errors);

            register.Password = "testtest";

            errors = new Registration.RegistrationValidator().Validate(register);
            Assert.Single(errors.Errors);

            register.Password = "12345678";

            errors = new Registration.RegistrationValidator().Validate(register);
            Assert.Single(errors.Errors);

            register.Password = "!@#$%^&*";

            errors = new Registration.RegistrationValidator().Validate(register);
            Assert.Single(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Bad_Email()
        {
            var register = new Registration
            {
                Username = "chris",
                Password = "aPas$tesT123!",
                Email = "christest.com"
            };

            var errors = new Registration.RegistrationValidator().Validate(register);
            Assert.Single(errors.Errors);

            // This is NOT a valid email address, FluentValidation
            // register.Email = "chris@test";

            // errors = new Registration.RegistrationValidator().Validate(register);
            // Assert.Single(errors.Errors);
        }
    }
}
