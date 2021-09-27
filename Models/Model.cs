using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    public class Model : BaseEntity
    {
        [JsonPropertyName("manufacturer")]
        public Manufacturer Manufacturer { get; set; }

        [JsonPropertyName("categoryClass")]
        public CategoryClass CategoryClass { get; set; }

        [JsonPropertyName("typeName")]
        public string TypeName { get; set; }

        [JsonPropertyName("family")]
        public string Family { get; set; }

        [JsonPropertyName("modelName")]
        public string ModelName { get; set; }

        [JsonPropertyName("isComplex")]
        public bool IsComplex { get; set; }

        [JsonPropertyName("IsHighPerformance")]
        public bool IsHighPerformance { get; set; }

        [JsonPropertyName("IsTailwheel")]
        public bool IsTailwheel { get; set; }

        [JsonPropertyName("HasConstantPropellor")]
        public bool HasConstantPropellor { get; set; }

        [JsonPropertyName("IsTurbine")]
        public bool IsTurbine { get; set; }

        [JsonPropertyName("IsCertifiedSinglePilot")]
        public bool IsCertifiedSinglePilot { get; set; }

        [JsonPropertyName("HasFlaps")]
        public bool HasFlaps { get; set; }

        [JsonPropertyName("IsSimOnly")]
        public bool IsSimOnly { get; set; }

        [JsonPropertyName("IsMotorGlider")]
        public bool IsMotorGlider { get; set; }

        [JsonPropertyName("IsMultiHelicopter")]
        public bool IsMultiHelicopter { get; set; }
    }
}