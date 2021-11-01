using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Nisshi.Models.Users;
using Nisshi.Requests.Users;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Users
{
    /// <summary>
    /// Tests registering a user in various scenarios
    /// </summary>
    public class RegisterTests : IDisposable
    {
        private readonly SliceFixture fixture;

        public RegisterTests()
        {
            this.fixture = new SliceFixture();
        }

        [Fact]
        public async Task Should_Have_Been_Registered()
        {
            var registration = new Registration
            {
                Username = Helpers.TestUserName,
                Email = Helpers.TestEmailAddress,
                Password = "Test123!"
            };

            var response = await fixture.SendAsync(new Register.Command(registration));

            Assert.NotNull(response);

            var fromDb = await fixture.GetNisshiContext().Users
                .Where(x => x.Username == response.Username).FirstOrDefaultAsync();

            Assert.NotNull(fromDb);

            Assert.Equal(fromDb.Username, response.Username);
            Assert.Equal(fromDb.Email, response.Email);
            Assert.NotEmpty(response.Token);
        }

        [Fact]
        public async Task Should_Fail_Input_Null()
        {
            await Assert.ThrowsAsync<ValidationException>(() => fixture.SendAsync(new Register.Command(null)));
        }

        [Fact]
        public async Task Should_Fail_Username_Exists()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            var registration = new Registration
            {
                Username = Helpers.TestUserName,
                Email = "test2@test.com",
                Password = "Test123!"
            };

            await Assert.ThrowsAsync<InvalidCredentialException>(() => fixture.SendAsync(new Register.Command(registration)));
        }

        [Fact]
        public async Task Should_Fail_Email_Exists()
        {
            var user = await Helpers.RegisterAndGetTestUser(fixture);

            var registration = new Registration
            {
                Username = "test2User",
                Email = Helpers.TestEmailAddress,
                Password = "Test123!"
            };

            await Assert.ThrowsAsync<InvalidCredentialException>(() => fixture.SendAsync(new Register.Command(registration)));
        }

        public void Dispose()
        {
            fixture.ResetDatabase();
        }
    }
}
