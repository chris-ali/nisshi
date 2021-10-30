using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentValidation;
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

        public class AircraftValidator : AbstractValidator<Aircraft>
        {
            public AircraftValidator()
            {
                RuleFor(x => x.TailNumber).NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(20).WithMessage("Length20");
                RuleFor(x => x.LastAnnual).LessThanOrEqualTo(DateTime.Now).WithMessage("PastDate")
                    .Unless(x => x.LastAnnual == null);
                RuleFor(x => x.LastPitotStatic).LessThanOrEqualTo(DateTime.Now).WithMessage("PastDate")
                    .Unless(x => x.LastPitotStatic == null);
                RuleFor(x => x.LastVOR).LessThanOrEqualTo(DateTime.Now).WithMessage("PastDate")
                    .Unless(x => x.LastVOR == null);
                RuleFor(x => x.LastAltimeter).LessThanOrEqualTo(DateTime.Now).WithMessage("PastDate")
                    .Unless(x => x.LastAltimeter == null);
                RuleFor(x => x.LastELT).LessThanOrEqualTo(DateTime.Now).WithMessage("PastDate")
                    .Unless(x => x.LastELT == null);
                RuleFor(x => x.LastTransponder).LessThanOrEqualTo(DateTime.Now).WithMessage("FutureDate")
                    .Unless(x => x.LastTransponder == null);
                RuleFor(x => x.RegistrationDue).GreaterThanOrEqualTo(DateTime.Now).WithMessage("FutureDate")
                    .Unless(x => x.RegistrationDue == null);
                RuleFor(x => x.Last100Hobbs).GreaterThanOrEqualTo(0).WithMessage("NonNegative");
                RuleFor(x => x.LastOilHobbs).GreaterThanOrEqualTo(0).WithMessage("NonNegative");
                RuleFor(x => x.LastEngineHobbs).GreaterThanOrEqualTo(0).WithMessage("NonNegative");
            }
        }
    }
}
