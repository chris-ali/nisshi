using FluentValidation;

namespace Nisshi.Models.Users
{
    /// <summary>
    /// Model that contains fundamental info for logging into the application
    /// </summary>
    public class Authenticate
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public class AuthenticateValidator : AbstractValidator<Authenticate>
        {
            public AuthenticateValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty().MaximumLength(128);
                RuleFor(x => x.Password).NotNull().NotEmpty().MaximumLength(20);
            }
        }
    }
}