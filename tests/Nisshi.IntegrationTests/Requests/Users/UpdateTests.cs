using System;
using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Enums;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models;
using Nisshi.Models.Users;
using Nisshi.Requests.Users;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Users
{
    /// <summary>
    /// Tests editing a user in various scenarios
    /// </summary>
    public class UpdateTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture fixture;

        public UpdateTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Have_Been_Updated()
        {
            var user = await Helpers.RegisterTestUser(fixture);

            var profileRequest = new Profile
            {
                Username = user.Username,
                Email = user.Email,
                Password = "test456!",
                CertificateNumber = "123456",
                CFIExpiration = DateTime.Today.AddMonths(12),
                IsInstructor = true,
                FirstName = "FirstName",
                LastBFR = DateTime.Today.AddMonths(-4),
                LastMedical = DateTime.Today.AddMonths(-5),
                LastName = "LastName",
                License = "TestLicense",
                MonthsToMedical = 5,
                PasswordAnswer = "Test answer.",
                PasswordQuestion = "Test question?",
                Preferences = "{option: value, option2: value2}"
            };

            var updateResponse = await fixture.SendAsync(new UpdateProfile.Command(profileRequest));

            Assert.NotNull(updateResponse);
            Assert.NotEqual(user.Hash, updateResponse.Hash);
            Assert.NotEqual(user.Salt, updateResponse.Salt);
            Assert.NotEqual(user.FirstName, updateResponse.FirstName);
            Assert.NotEqual(user.LastBFR, updateResponse.LastBFR);
            Assert.NotEqual(user.MonthsToMedical, updateResponse.MonthsToMedical);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            var user = await Helpers.RegisterTestUser(fixture);
            
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new UpdateProfile.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_No_User()
        {
            var profileRequest = new Profile
            {
                Username = "nobody",
                Email = "nobody@test.com",
                Password = "test456!",
                CertificateNumber = "123456",
                CFIExpiration = DateTime.Today.AddMonths(12),
                IsInstructor = true,
                FirstName = "FirstName",
                LastBFR = DateTime.Today.AddMonths(-4),
                LastMedical = DateTime.Today.AddMonths(-5),
                LastName = "LastName",
                License = "TestLicense",
                MonthsToMedical = 5,
                PasswordAnswer = "Test answer.",
                PasswordQuestion = "Test question?",
                Preferences = "{option: value, option2: value2}"
            };

            await Assert.ThrowsAsync<RestException>(() => fixture.SendAsync(new UpdateProfile.Command(profileRequest)));
        }
    }
}
