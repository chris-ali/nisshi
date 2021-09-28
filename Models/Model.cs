using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    /// <summary>
    /// Representation of a model of aircraft
    /// </summary>
    public class Model : BaseEntity
    {
        /// <summary>
        /// The manufacturer of this model
        /// </summary>
        public Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// The category and class of this model
        /// </summary>
        public CategoryClass CategoryClass { get; set; }

        /// <summary>
        /// The type name of this aircraft 
        /// </summary>
        /// <value></value>
        public string TypeName { get; set; }

        /// <summary>
        /// The family for the model 
        /// (e.g., PA28-160 and PA28-180 are both PA28s)
        /// </summary>
        public string Family { get; set; }

        /// <summary>
        /// The name of this model
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Is the model a complex aircraft
        /// </summary>
        public bool IsComplex { get; set; }

        /// <summary>
        /// Is the model multi-engine
        /// </summary>
        public bool IsMultiEngine { get; set; }

        /// <summary>
        /// Is the model high performance
        /// </summary>
        public bool IsHighPerformance { get; set; }

        /// <summary>
        /// Is the model a taildragger
        /// </summary>
        public bool IsTailwheel { get; set; }

        /// <summary>
        /// Does the model have constant speed propeller(s)
        /// </summary>
        public bool HasConstantPropeller { get; set; }

        /// <summary>
        /// Does the model have turbine engines
        /// </summary>
        public bool IsTurbine { get; set; }

        /// <summary>
        /// For type-rated turbine aircraft, 
        /// indicates certification for single-pilot operations
        /// </summary>
        public bool IsCertifiedSinglePilot { get; set; }

        /// <summary>
        /// Does the model have flaps
        /// </summary>
        public bool HasFlaps { get; set; }

        /// <summary>
        /// If true, all aircraft of this make/model are simulated
        /// </summary>
        /// <value></value>
        public bool IsSimOnly { get; set; }

        /// <summary>
        /// Is the model a motorglider
        /// </summary>
        /// <value></value>
        public bool IsMotorGlider { get; set; }

        /// <summary>
        /// Is the aircraft a helicopter
        /// </summary>
        public bool IsHelicopter { get; set; }

        /// <summary>
        /// Foreign key to the categoryclass table
        /// </summary>
        /// <value></value>
        public int IDCategoryClass { get; set; }

        /// <summary>
        /// Foreign key to the manufacturers table
        /// </summary>
        /// <value></value>
        public int IDManufacturer { get; set; }

        /// <summary>
        /// Associated aircraft with this model
        /// </summary>
        [JsonIgnore]
        public virtual List<Aircraft> Aircraft { get; set; }
    }
}