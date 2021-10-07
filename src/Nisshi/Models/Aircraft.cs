using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Nisshi.Infrastructure.Enums;
using Nisshi.Models.Users;

namespace Nisshi.Models
{
    /// <summary>
    /// Aircraft (either real or simulated) that carried out a flight in a logbook entry
    /// </summary>
    public class Aircraft : BaseEntity
    {
        public string TailNumber { get; set; }

        /// <summary>
        /// InstanceType of this airplane (real (1) vs. simulator (2))
        /// </summary>
        public InstanceType InstanceType { get; set; }

        /// <summary>
        /// Date of last annual inspection
        /// </summary>
        public DateTime? LastAnnual { get; set; }

        /// <summary>
        /// Date of last pitot static inspection
        /// </summary>
        public DateTime? LastPitotStatic { get; set; }

        /// <summary>
        /// Date of last VOR inspection
        /// </summary>
        public DateTime? LastVOR { get; set; }

        /// <summary>
        /// Date of last altimeter inspection
        /// </summary>
        public DateTime? LastAltimeter { get; set; }

        /// <summary>
        /// Date of last transponder inspection
        /// </summary>
        public DateTime? LastTransponder { get; set; }

        /// <summary>
        /// Date of last ELT inspection
        /// </summary>
        public DateTime? LastELT { get; set; }

        /// <summary>
        /// Hobbs time of last 100 hour inspection
        /// </summary>
        public decimal Last100Hobbs { get; set; }

        /// <summary>
        /// Hobbs time of last engine oil change
        /// </summary>
        public decimal LastOilHobbs { get; set; }

        /// <summary>
        /// Last recorded engine time
        /// </summary>
        public decimal LastEngineHobbs { get; set; }

        /// <summary>
        /// Date, if any, of next renewal of registration
        /// </summary>
        public DateTime? RegistrationDue { get; set; }

        /// <summary>
        /// Notes about the aircraft
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Foreign key to the models table
        /// </summary>
        public int IdModel { get; set; }

        /// <summary>
        /// Associated model of this aircraft
        /// </summary>
        public Model Model { get; set; }
        
        /// <summary>
        /// Associated logbook entries of this aircraft 
        /// </summary>
        [JsonIgnore]
        public virtual List<LogbookEntry> LogbookEntries { get; set; } = new();
        
        /// <summary>
        /// Foreign key to the users table
        /// </summary>
        [JsonIgnore]
        public int IdUser { get; set; }

        /// <summary>
        /// Owner of this aircraft
        /// </summary>
        [JsonIgnore]
        public virtual User Owner { get; set; }
    }
}