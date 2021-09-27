using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    /// <summary>
    /// Aircraft (either real or simulated) that carried out a flight in a logbook entry
    /// </summary>
    public class Aircraft : BaseEntity
    {
        /// <summary>
        /// Associated model of this aircraft
        /// </summary>
        [JsonPropertyName("model")]
        public Model Model { get; set; }

        [JsonPropertyName("tailNumber")]
        public string TailNumber { get; set; }

        /// <summary>
        /// InstanceType of this airplane (real (1) vs. simulator (0))
        /// </summary>
        [JsonPropertyName("instanceType")]
        public int InstanceType { get; set; }

        /// <summary>
        /// Date of last annual inspection
        /// </summary>
        [JsonPropertyName("lastAnnual")]
        public DateTime LastAnnual { get; set; }

        /// <summary>
        /// Date of last pitot static inspection
        /// </summary>
        [JsonPropertyName("lastPitotStatic")]
        public DateTime LastPitotStatic { get; set; }

        /// <summary>
        /// Date of last VOR inspection
        /// </summary>
        [JsonPropertyName("lastVOR")]
        public DateTime LastVOR { get; set; }

        /// <summary>
        /// Date of last altimeter inspection
        /// </summary>
        [JsonPropertyName("lastAltimeter")]
        public DateTime LastAltimeter { get; set; }

        /// <summary>
        /// Date of last transponder inspection
        /// </summary>
        [JsonPropertyName("lastTransponder")]
        public DateTime LastTransponder { get; set; }

        /// <summary>
        /// Date of last ELT inspection
        /// </summary>
        [JsonPropertyName("lastELT")]
        public DateTime LastELT { get; set; }

        /// <summary>
        /// Hobbs time of last 100 hour inspection
        /// </summary>
        [JsonPropertyName("last100")]
        public decimal Last100Hobbs { get; set; }

        /// <summary>
        /// Hobbs time of last engine oil change
        /// </summary>
        [JsonPropertyName("lastOil")]
        public decimal LastOilHobbs { get; set; }

        /// <summary>
        /// Last recorded engine time
        /// </summary>
        [JsonPropertyName("lastEngine")]
        public decimal LastEngineHobbs { get; set; }

        /// <summary>
        /// Date, if any, of next renewal of registration
        /// </summary>
        /// <value></value>
        [JsonPropertyName("registrationDue")]
        public DateTime RegistrationDue { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        /// <summary>
        /// Associated logbook entries of this aircraft 
        /// </summary>
        /// <returns></returns>
        [JsonIgnore]
        public virtual List<User> LogbookEntries { get; set; } = new();
        
        /// <summary>
        /// Owner of this aircraft
        /// </summary>
        [JsonIgnore]
        public virtual User Owner { get; set; }
    }
}