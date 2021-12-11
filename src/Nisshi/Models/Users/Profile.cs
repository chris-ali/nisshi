using System;
using FluentValidation;

namespace Nisshi.Models.Users
{
    /// <summary>
    /// Model passed in to controller to edit the profile and password
    /// </summary>
    public class Profile
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// Question to reset user's password
        /// </summary>
        public string PasswordQuestion { get; set; }

        /// <summary>
        /// Answer to reset user's password
        /// </summary>
        public string PasswordAnswer { get; set; }

        /// <summary>
        /// Date of last BFR
        /// </summary>
        public DateTime? LastBFR { get; set; }

        /// <summary>
        /// Date of last medical examination
        /// </summary>
        public DateTime? LastMedical { get; set; }

        /// <summary>
        /// How many months until current medical examination expires
        /// </summary>
        public int MonthsToMedical { get; set; }

        /// <summary>
        /// Is this user a CFI
        /// </summary>
        public bool IsInstructor { get; set; }

        /// <summary>
        /// Expiration of certificate
        /// </summary>
        public DateTime? CFIExpiration { get; set; }

        /// <summary>
        /// Pilot license type
        /// </summary>
        public string License { get; set; }

        /// <summary>
        /// Pilot certificate number
        /// </summary>
        public string CertificateNumber { get; set; }

        public class ProfileValidator : AbstractValidator<Profile>
        {
            public ProfileValidator()
            {
                // 8-20 characters, at least one lower case, one upper case, one number, one special
                var passRegex = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,20}$";

                RuleFor(x => x.Username).NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(60).WithMessage("Length60");
                RuleFor(x => x.Password).Matches(passRegex).WithMessage("InvalidPassword")
                    .Unless(x => string.IsNullOrEmpty(x.Password));
                RuleFor(x => x.Email).MaximumLength(60).WithMessage("Length60")
                    .EmailAddress().WithMessage("InvalidEmail");
                RuleFor(x => x.FirstName).MaximumLength(60).WithMessage("Length60");
                RuleFor(x => x.LastName).MaximumLength(60).WithMessage("Length60");
                RuleFor(x => x.PasswordQuestion)
                    //.NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(200).WithMessage("Length200");
                RuleFor(x => x.PasswordAnswer)
                    //.NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(200).WithMessage("Length200");
                RuleFor(x => x.LastBFR).LessThanOrEqualTo(DateTime.Now).WithMessage("PastDate")
                    .Unless(x => x.LastBFR == null);
                RuleFor(x => x.LastMedical).LessThanOrEqualTo(DateTime.Now).WithMessage("PastDate")
                    .Unless(x => x.LastMedical == null);
                RuleFor(x => x.CFIExpiration).GreaterThanOrEqualTo(DateTime.Now).WithMessage("FutureDate")
                    .Unless(x => x.CFIExpiration == null);
                RuleFor(x => x.License).Length(0, 45).WithMessage("Length45");
                RuleFor(x => x.CertificateNumber).Length(0, 45).WithMessage("Length45");
                RuleFor(x => x.MonthsToMedical).GreaterThanOrEqualTo(0).WithMessage("NonNegative");
            }
        }
    }
}
