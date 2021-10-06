using System;
using Nisshi.Infrastructure.Enums;

namespace Nisshi.Models.Users
{
    /// <summary>
    /// Provides user profile information and token data when a user logs in successfully
    /// </summary>
    public class LoggedIn
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public DateTime LastLoginDate { get; set; }

        public bool IsLockedOut { get; set; }

        public UserType UserType { get; set; }

        /// <summary>
        /// Date of last BFR
        /// </summary>
        public DateTime LastBFR { get; set; }

        /// <summary>
        /// Date of last medical examination
        /// </summary>
        public DateTime LastMedical { get; set; }

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
        public DateTime CFIExpiration { get; set; }

        /// <summary>
        /// Pilot license type
        /// </summary>
        public string License { get; set; }

        /// <summary>
        /// Pilot certificate number
        /// </summary>
        public string CertificateNumber { get; set; }

        /// <summary>
        /// Additional user preferences serialized in JSON dictionary format
        /// </summary>
        public string Preferences { get; set; }

        /// <summary>
        /// JWT token generated after logging in successfully
        /// </summary>
        public string Token { get; set; }
    }
}