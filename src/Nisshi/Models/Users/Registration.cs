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
                RuleFor(x => x.Username).NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(60).WithMessage("Length60");
                RuleFor(x => x.Password).NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(20).WithMessage("InvalidPassword");
                RuleFor(x => x.Email).EmailAddress().WithMessage("InvalidEmail")
                    .MaximumLength(60).WithMessage("Length60");
            }
        }
    }
}
