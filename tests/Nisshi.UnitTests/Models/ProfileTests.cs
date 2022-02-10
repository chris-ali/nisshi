using System;
using Nisshi.Models.Users;
using Xunit;

namespace Nisshi.UnitTests.Models
{
    /// <summary>
    /// Tests profile validation in various scenarios
    /// </summary>
    public class ProfileTests
    {
        [Fact]
        public void Should_Pass_Validation()
        {
            var profile = new Profile
            {
                Username = "chris",
                Email = "chris@test.com",
                CertificateNumber = "123456789",
                License = "Test License",
                CFIExpiration = DateTime.Now.AddMonths(2),
                FirstName = "Chris",
                LastName = "Ali",
                LastBFR = DateTime.Now.AddMonths(-2),
                LastMedical = DateTime.Now.AddMonths(-6),
                MonthsToMedical = 10,
                PasswordQuestion = "Test question?",
                PasswordAnswer = "Test answer."
            };

            var errors = new Profile.ProfileValidator().Validate(profile);
            Assert.Empty(errors.Errors);

            profile.LastBFR = profile.LastMedical = profile.CFIExpiration = null;
            profile.MonthsToMedical = 0;

            errors = new Profile.ProfileValidator().Validate(profile);
            Assert.Empty(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Too_Long_Too_Short_Empty()
        {
            var profile = new Profile
            {
                Username = "christophechristophechristophechristophechristophechristophechristophe",
                Email = "christophechristophechristophechristophechristophechristophechristophe@test.com",
                CertificateNumber = "christophechristophechristophechristophechristophechristophe",
                License = "christophechristophechristophechristophechristophechristophechristophe",
                CFIExpiration = DateTime.Now.AddMonths(2),
                FirstName = "christophechristophechristophechristophechristophechristophechristophe",
                LastName = "christophechristophechristophechristophechristophechristophechristophe",
                LastBFR = DateTime.Now.AddMonths(-2),
                LastMedical = DateTime.Now.AddMonths(-6),
                MonthsToMedical = 10,
                PasswordQuestion = "christophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophe",
                PasswordAnswer = "christophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophechristophe"
            };

            var errors = new Profile.ProfileValidator().Validate(profile);
            Assert.Equal(8, errors.Errors.Count);

            profile.Username = "";
            profile.Email = "";

            errors = new Profile.ProfileValidator().Validate(profile);
            Assert.Equal(8, errors.Errors.Count);
        }

        [Fact]
        public void Should_Fail_Validation_Bad_Numerics()
        {
            var profile = new Profile
            {
                Username = "chris",
                Email = "chris@test.com",
                CertificateNumber = "123456789",
                License = "Test License",
                CFIExpiration = DateTime.Now.AddMonths(-2),
                FirstName = "Chris",
                LastName = "Ali",
                LastBFR = DateTime.Now.AddMonths(2),
                LastMedical = DateTime.Now.AddMonths(6),
                MonthsToMedical = -10,
                PasswordQuestion = "Test question?",
                PasswordAnswer = "Test answer."
            };

            var errors = new Profile.ProfileValidator().Validate(profile);
            Assert.Equal(4, errors.Errors.Count);
        }

        [Fact]
        public void Should_Fail_Validation_Bad_Email()
        {
            var profile = new Profile
            {
                Username = "chris",
                Email = "christest.com",
                CertificateNumber = "123456789",
                License = "Test License",
                CFIExpiration = DateTime.Now.AddMonths(2),
                FirstName = "Chris",
                LastName = "Ali",
                LastBFR = DateTime.Now.AddMonths(-2),
                LastMedical = DateTime.Now.AddMonths(-6),
                MonthsToMedical = 10,
                PasswordQuestion = "Test question?",
                PasswordAnswer = "Test answer."
            };

            var errors = new Profile.ProfileValidator().Validate(profile);
            Assert.Single(errors.Errors);

            // This is NOT a valid email address, FluentValidation
            // profile.Email = "chris@test";

            // errors = new Profile.ProfileValidator().Validate(profile);
            // Assert.Single(errors.Errors);
        }
    }
}
