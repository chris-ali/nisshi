using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models 
{
    /// <summary>
    /// User object for project, tied to users table
    /// </summary>
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Hashed and salted password for this user
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("passwordQuestion")]
        public string PasswordQuestion { get; set; }

        [JsonPropertyName("passwordAnswer")]
        public string PasswordAnswer { get; set; }

        [JsonPropertyName("lastPasswordChangedDate")]
        public DateTime LastPasswordChangedDate { get; set; }

        [JsonPropertyName("lastLoginDate")]
        public DateTime LastLoginDate { get; set; }

        [JsonPropertyName("isLockedOut")]
        public bool IsLockedOut { get; set; }

        [JsonPropertyName("lastLockedOutDate")]
        public DateTime LastLockedOutDate { get; set; }

        [JsonPropertyName("lastBFR")]
        public DateTime LastBFR { get; set; }

        [JsonPropertyName("lastMedical")]
        public DateTime LastMedical { get; set; }

        [JsonPropertyName("monthsToMedical")]
        public int MonthsToMedical { get; set; }

        [JsonPropertyName("isInstructor")]
        public bool IsInstructor { get; set; }

        [JsonPropertyName("CFIExpiration")]
        public DateTime CFIExpiration { get; set; }

        [JsonPropertyName("license")]
        public string License { get; set; }

        [JsonPropertyName("certificateNumber")]
        public string CertificateNumber { get; set; }

        [JsonPropertyName("timeZone")]
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