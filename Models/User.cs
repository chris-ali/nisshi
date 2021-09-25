using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models 
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [JsonIgnore]
        public virtual List<LogMessage> Messages { get; set; }

        [JsonIgnore]
        public virtual List<Hero> Heroes {get; set;}

        [JsonIgnore]
        public virtual List<HeroUser> HeroUsers { get; set; }
    }
}