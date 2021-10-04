using FluentValidation;

namespace Nisshi.Models
{
    /// <summary>
    /// Model that contains fundamental info for logging into the application
    /// </summary>
    public class UserLogin
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public class RegistrationValidator : AbstractValidator<UserLogin>
        {
            public RegistrationValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty().MaximumLength(128);
                RuleFor(x => x.Password).NotNull().NotEmpty().MaximumLength(20);
            }
        }
    }
}