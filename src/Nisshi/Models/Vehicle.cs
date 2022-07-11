using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentValidation;
using Nisshi.Models.Users;

namespace Nisshi.Models
{
    /// <summary>
    /// Vehicle entity
    /// </summary>
    public class Vehicle : BaseEntity
    {
        /// <summary>
        /// VIN of the vehicle
        /// </summary>
        public string Vin { get; set; }

        /// <summary>
        /// Make of the vehicle
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// Model of the vehicle
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Trim level of the vehicle
        /// </summary>
        public string Trim { get; set; }

        /// <summary>
        /// Year of the vehicle
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Date of last annual inspection
        /// </summary>
        public DateTime? LastRegistration { get; set; }

        /// <summary>
        /// Date, if any, of next renewal of registration
        /// </summary>
        public DateTime? RegistrationDue { get; set; }

        /// <summary>
        /// Date of last pitot static inspection
        /// </summary>
        public DateTime? LastInspection { get; set; }

        /// <summary>
        /// Date, if any, of next renewal of inspection
        /// </summary>
        public DateTime? InspectionDue { get; set; }

        /// <summary>
        /// Miles currently on the vehicle
        /// </summary>
        public decimal Miles { get; set; }

        /// <summary>
        /// Notes about the vehicle
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Associated maintenance entries of this aircraft
        /// </summary>
        [JsonIgnore]
        public virtual List<MaintenanceEntry> MaintenanceEntries { get; set; } = new();

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

        public class VehicleValidator : AbstractValidator<Vehicle>
        {
            public VehicleValidator()
            {
                RuleFor(x => x.Vin).MaximumLength(20).WithMessage("Length20");
                RuleFor(x => x.Make).NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(45).WithMessage("Length45");
                RuleFor(x => x.Model).NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(45).WithMessage("Length45");
                RuleFor(x => x.Trim).NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(45).WithMessage("Length45");
                RuleFor(x => x.Year).GreaterThanOrEqualTo(0).WithMessage("NonNegative");
                RuleFor(x => x.Miles).GreaterThanOrEqualTo(0).WithMessage("NonNegative");
                RuleFor(x => x.LastInspection).LessThanOrEqualTo(DateTime.Now).WithMessage("NotFutureDate")
                    .Unless(x => x.LastInspection == null);
                RuleFor(x => x.LastRegistration).LessThanOrEqualTo(DateTime.Now).WithMessage("NotFutureDate")
                    .Unless(x => x.LastRegistration == null);
                RuleFor(x => x.Notes).MaximumLength(200).WithMessage("Length200");
            }
        }
    }
}
