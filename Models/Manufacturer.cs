using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    public class Manufacturer : BaseEntity
    {
        [JsonPropertyName("manufacturerName")]
        public string ManufacturerName { get; set; }

        [JsonPropertyName("models")]
        public virtual List<Model> Models { get; set; } = new();
    }
}