using Nisshi.Models.Users;
using Xunit;

namespace Nisshi.UnitTests.Models
{
    /// <summary>
    /// Tests authentication validation in various scenarios
    /// </summary>
    public class AuthenticateTests
    {
        [Fact]
        public void Should_Pass_Validation()
        {
            var authenticate = new Authenticate
            {
                Username = "chris",
                Password = "aPas$tesT123!"
            };

            var errors = new Authenticate.AuthenticateValidator().Validate(authenticate);
            Assert.Empty(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Too_Long_Empty()
        {
            var authenticate = new Authenticate
            {
                Username = "christophechristophechristophechristophechristophechristophechristophe",
                Password = "tesT123!tesT123!tesT123!"
            };

            var errors = new Authenticate.AuthenticateValidator().Validate(authenticate);
            Assert.Equal(2, errors.Errors.Count);

            authenticate.Username = "";
            authenticate.Password = "";

            errors = new Authenticate.AuthenticateValidator().Validate(authenticate);
            Assert.Equal(2, errors.Errors.Count);
        }
    }
}
