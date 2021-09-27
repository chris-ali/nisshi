using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    /// <summary>
    /// Category and class designation for a model of aircraft
    /// </summary>
    public class CategoryClass
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Aircraft Category and Class
        /// </summary>
        [JsonPropertyName("catClass")]
        public string CatClass { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }

        /// <summary>
        /// Secondary category/class for airplanes that are part-time on floats or amphibious
        /// </summary>
        [JsonPropertyName("alternateCatClass")]
        public int AlternateCatClass { get; set; }
    }
}