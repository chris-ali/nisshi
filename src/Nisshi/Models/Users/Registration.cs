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
                // 8-20 characters, at least one lower case, one upper case, one number, one special
                var passRegex = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,20}$";

                RuleFor(x => x.Username).NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(60).WithMessage("Length60");
                RuleFor(x => x.Password).Matches(passRegex).WithMessage("InvalidPassword");
                RuleFor(x => x.Email).EmailAddress().WithMessage("InvalidEmail")
                    .MaximumLength(60).WithMessage("Length60");
            }
        }
    }
}
