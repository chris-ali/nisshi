using FluentValidation;

namespace Nisshi.Models.Users
{
    /// <summary>
    /// Model that contains fundamental info for registering a new user
    /// </summary>
    public class Registration
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public class RegistrationValidator : AbstractValidator<Registration>
        {
            public RegistrationValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty().MaximumLength(128);
                RuleFor(x => x.Password).NotNull().NotEmpty().MaximumLength(20);
                RuleFor(x => x.Email).NotNull().EmailAddress().MaximumLength(100);
            }
        }
    }
}