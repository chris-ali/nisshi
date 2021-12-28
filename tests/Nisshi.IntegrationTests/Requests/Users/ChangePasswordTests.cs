using System;
using System.Security.Authentication;
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
    public class ChangePasswordTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public ChangePasswordTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Should_Have_Been_Updated()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            var changeRequest = new ChangePasswordModel
            {
                OldPassword = "Test123!",
                NewPassword = "Test124!",
                RepeatPassword = "Test124!",
            };

            var updateResponse = await fixture.SendAsync(new ChangePassword.Command(changeRequest));

            Assert.NotNull(updateResponse);

            // Need to call a request, no idea why EF context.Find() doesn't work for User
            var fromDb = await fixture.SendAsync(new GetCurrent.Query());

            Assert.NotNull(fromDb);

            Assert.NotEqual(user.Hash, updateResponse.Hash);
            Assert.NotEqual(user.Salt, updateResponse.Salt);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new ChangePassword.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_No_User()
        {
            var profileRequest = new ChangePasswordModel
            {
                OldPassword = "Test123!",
                NewPassword = "Test124!",
                RepeatPassword = "Test124!",
            };

            await Assert.ThrowsAsync<DomainException>(() => fixture.SendAsync(new ChangePassword.Command(profileRequest)));
        }

        [Fact]
        public async Task Should_Fail_Old_Password_Doesnt_Match()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            var profileRequest = new ChangePasswordModel
            {
                OldPassword = "Test125!",
                NewPassword = "Test124!",
                RepeatPassword = "Test124!",
            };

            await Assert.ThrowsAsync<AuthenticationException>(() => fixture.SendAsync(new ChangePassword.Command(profileRequest)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
