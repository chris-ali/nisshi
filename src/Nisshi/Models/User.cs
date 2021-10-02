using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Nisshi.Infrastructure.Enums;

namespace Nisshi.Models 
{
    /// <summary>
    /// User object for project, tied to users table
    /// </summary>
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        /// <summary>
        /// Hashed and salted password for this user
        /// </summary>
        [JsonIgnore]
        public string Password { get; set; }

        /// <summary>
        /// Question to reset user's password
        /// </summary>
        public string PasswordQuestion { get; set; }

        /// <summary>
        /// Answer to reset user's password
        /// </summary>
        [JsonIgnore]
        public string PasswordAnswer { get; set; }

        /// <summary>
        /// Date the password was last changed
        /// </summary>
        public DateTime LastPasswordChangedDate { get; set; }

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
        /// Additional user preferences serialized in JSON dictionary forma
        /// </summary>
        public virtual string Preferences { get; set; }
        
        /// <summary>
        /// Associated aircraft belonging to the user
        /// </summary>
        [JsonIgnore]
        public virtual List<Aircraft> Aircraft { get; set; }

        /// <summary>
        /// Associated logbook entries belonging to the user
        /// </summary>
        [JsonIgnore]
        public virtual List<LogbookEntry> LogbookEntries { get; set; }
    }
}