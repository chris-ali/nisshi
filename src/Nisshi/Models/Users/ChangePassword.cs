using System;
using FluentValidation;

namespace Nisshi.Models.Users
{
    /// <summary>
    /// Model passed in to controller to edit the password
    /// </summary>
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string RepeatPassword { get; set; }

        /// <summary>
        /// Question to reset user's password
        /// </summary>
        public string PasswordQuestion { get; set; }

        /// <summary>
        /// Answer to reset user's password
        /// </summary>
        public string PasswordAnswer { get; set; }

        public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
        {
            public ChangePasswordValidator()
            {
                // 8-20 characters, at least one lower case, one upper case, one number, one special
                var passRegex = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,20}$";

                RuleFor(x => x.NewPassword).NotEmpty().Matches(passRegex).WithMessage("InvalidPassword");

                RuleFor(x => x.RepeatPassword).NotEmpty().Equal(x => x.NewPassword).WithMessage("InvalidPassword");

                RuleFor(x => x.OldPassword).NotEmpty().NotEqual(x => x.NewPassword).WithMessage("InvalidPassword");
            }
        }
    }
}
