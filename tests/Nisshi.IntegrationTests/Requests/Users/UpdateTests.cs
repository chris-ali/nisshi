using System;
using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models.Users;
using Nisshi.Requests.Users;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Users
{
    /// <summary>
    /// Tests editing a user in various scenarios
    /// </summary>
    public class UpdateTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public UpdateTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Should_Have_Been_Updated()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

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

            // Need to call a request, no idea why EF context.Find() doesn't work for User 
            var fromDb = await fixture.SendAsync(new GetCurrent.Query());

            Assert.NotNull(fromDb);

            Assert.NotEqual(user.Hash, updateResponse.Hash);
            Assert.NotEqual(user.Salt, updateResponse.Salt);
            Assert.Equal(fromDb.FirstName, updateResponse.FirstName);
            Assert.Equal(fromDb.LastName, updateResponse.LastName);
            Assert.Equal(fromDb.LastBFR, updateResponse.LastBFR);
            Assert.Equal(fromDb.MonthsToMedical, updateResponse.MonthsToMedical);
            Assert.Equal(fromDb.CertificateNumber, updateResponse.CertificateNumber);
            Assert.Equal(fromDb.CFIExpiration, updateResponse.CFIExpiration);
            Assert.Equal(fromDb.IsInstructor, updateResponse.IsInstructor);
            Assert.Equal(fromDb.LastMedical, updateResponse.LastMedical);
            Assert.Equal(fromDb.License, updateResponse.License);
            Assert.Equal(fromDb.PasswordAnswer, updateResponse.PasswordAnswer);
            Assert.Equal(fromDb.PasswordQuestion, updateResponse.PasswordQuestion);
            Assert.Equal(fromDb.Preferences, updateResponse.Preferences);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

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

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new UpdateProfile.Command(profileRequest)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
