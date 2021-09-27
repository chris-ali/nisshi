using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        public string Password { get; set; }

        public string PasswordQuestion { get; set; }

        public string PasswordAnswer { get; set; }

        public DateTime LastPasswordChangedDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public bool IsLockedOut { get; set; }

        public DateTime LastLockedOutDate { get; set; }

        public DateTime LastBFR { get; set; }

        public DateTime LastMedical { get; set; }

        public int MonthsToMedical { get; set; }

        public bool IsInstructor { get; set; }

        public DateTime CFIExpiration { get; set; }

        public string License { get; set; }

        public string CertificateNumber { get; set; }

        public string TimeZone { get; set; }

        [JsonIgnore]
        public virtual Dictionary<string, string> Preferences { get; set; }
        
        [JsonIgnore]
        public virtual List<Aircraft> Aircraft { get; set; }

        [JsonIgnore]
        public virtual List<LogbookEntry> LogbookEntries {get; set;}

        [JsonIgnore]
        public virtual List<HeroUser> HeroUsers { get; set; }
    }
}