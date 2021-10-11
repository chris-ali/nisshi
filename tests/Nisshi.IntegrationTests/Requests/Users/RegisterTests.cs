using System.Threading.Tasks;
using FluentValidation;
using Nisshi.Infrastructure.Errors;
using Nisshi.Models.Users;
using Nisshi.Requests.Users;
using Xunit;

namespace Nisshi.IntegrationTests.Requests.Users
{
    /// <summary>
    /// Tests registering a user in various scenarios
    /// </summary>
    public class RegisterTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture fixture;

        public RegisterTests(SliceFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Should_Have_Been_Registered()
        {
            var registration = new Registration
            {
                Username = Helpers.TestUserName,
                Email = Helpers.TestEmailAddress,
                Password = "test123!"
            };

            var response = await fixture.SendAsync(new Register.Command(registration));

            Assert.NotNull(response);
            Assert.Equal(registration.Username, response.Username);
            Assert.Equal(registration.Email, response.Email);
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
            var user = await Helpers.RegisterTestUser(fixture);

            var registration = new Registration
            {
                Username = Helpers.TestUserName,
                Email = "test2@test.com",
                Password = "test123!"
            };

            await Assert.ThrowsAsync<RestException>(() => fixture.SendAsync(new Register.Command(registration)));
        }

        [Fact]
        public async Task Should_Fail_Email_Exists()
        {
            var user = await Helpers.RegisterTestUser(fixture);

            var registration = new Registration
            {
                Username = "test2User",
                Email = Helpers.TestEmailAddress,
                Password = "test123!"
            };

            await Assert.ThrowsAsync<RestException>(() => fixture.SendAsync(new Register.Command(registration)));
        }
    }
}
