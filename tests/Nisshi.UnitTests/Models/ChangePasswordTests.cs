using System;
using Nisshi.Models.Users;
using Xunit;

namespace Nisshi.UnitTests.Models
{
    /// <summary>
    /// Tests changing password in various scenarios
    /// </summary>
    public class ChangePasswordModelTests
    {
        [Fact]
        public void Should_Pass_Validation()
        {
            var change = new ChangePasswordModel
            {
                OldPassword = "Test1234!",
                NewPassword = "Test1235!",
                RepeatPassword = "Test1235!",
                PasswordQuestion = "Test question?",
                PasswordAnswer = "Test answer."
            };

            var errors = new ChangePasswordModel.ChangePasswordValidator().Validate(change);
            Assert.Empty(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Bad_New_Password()
        {
            var change = new ChangePasswordModel
            {
                OldPassword = "test1234!",
                PasswordQuestion = "Test question?",
                PasswordAnswer = "Test answer."
            };

            var errors = new ChangePasswordModel.ChangePasswordValidator().Validate(change);
            Assert.Equal(2, errors.Errors.Count);

            change.NewPassword = change.RepeatPassword = "test1234";

            errors = new ChangePasswordModel.ChangePasswordValidator().Validate(change);
            Assert.Single(errors.Errors);

            change.NewPassword = change.RepeatPassword = "testtest";

            errors = new ChangePasswordModel.ChangePasswordValidator().Validate(change);
            Assert.Single(errors.Errors);

            change.NewPassword = change.RepeatPassword = "12345678";

            errors = new ChangePasswordModel.ChangePasswordValidator().Validate(change);
            Assert.Single(errors.Errors);

            change.NewPassword = change.RepeatPassword = "!@#$%^&*";

            errors = new ChangePasswordModel.ChangePasswordValidator().Validate(change);
            Assert.Single(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Passwords_Dont_Match()
        {
            var change = new ChangePasswordModel
            {
                OldPassword = "Test1234!",
                NewPassword = "Test1235!",
                RepeatPassword = "Test1236!",
                PasswordQuestion = "Test question?",
                PasswordAnswer = "Test answer."
            };

            var errors = new ChangePasswordModel.ChangePasswordValidator().Validate(change);
            Assert.Single(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_New_And_Old_Passwords_Match()
        {
            var change = new ChangePasswordModel
            {
                OldPassword = "Test1234!",
                NewPassword = "Test1234!",
                RepeatPassword = "Test1234!",
                PasswordQuestion = "Test question?",
                PasswordAnswer = "Test answer."
            };

            var errors = new ChangePasswordModel.ChangePasswordValidator().Validate(change);
            Assert.Single(errors.Errors);
        }
    }
}
