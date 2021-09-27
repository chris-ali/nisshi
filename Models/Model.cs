using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    public class Model : BaseEntity
    {
        public Manufacturer Manufacturer { get; set; }

        public CategoryClass CategoryClass { get; set; }

        public string TypeName { get; set; }

        public string Family { get; set; }

        public string ModelName { get; set; }

        public bool IsComplex { get; set; }

        public bool IsHighPerformance { get; set; }

        public bool IsTailwheel { get; set; }

        public bool HasConstantPropeller { get; set; }

        public bool IsTurbine { get; set; }

        public bool IsCertifiedSinglePilot { get; set; }

        public bool HasFlaps { get; set; }

        public bool IsSimOnly { get; set; }

        public bool IsMotorGlider { get; set; }

        public bool IsMultiHelicopter { get; set; }
    }
}