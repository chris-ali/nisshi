using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    public class Hero : BaseEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("power")]
        public string Power { get; set; }

        [JsonPropertyName("realName")]
        public string RealName { get; set; }

        [JsonIgnore]
        public virtual List<User> Users { get; set; } = new();
        
        [JsonIgnore]
        public virtual List<HeroUser> HeroUsers { get; set; } = new();
    }
}