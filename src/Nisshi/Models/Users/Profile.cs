using System;

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

        /// <summary>
        /// Additional user preferences serialized in JSON dictionary forma
        /// </summary>
        public string Preferences { get; set; }
    }
}