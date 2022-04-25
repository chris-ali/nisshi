using System;
using System.Text.Json.Serialization;
using FluentValidation;
using Nisshi.Infrastructure.Enums;
using Nisshi.Models.Users;

namespace Nisshi.Models
{
    /// <summary>
    /// A logged flight
    /// </summary>
    public class MaintenanceEntry : BaseEntity
    {
        /// <summary>
        /// Date that the maintenance took place
        /// </summary>
        public DateTime? DatePerformed { get; set; }

        /// <summary>
        /// Miles performed at
        /// </summary>
        public decimal MilesPerformed { get; set; }

        /// <summary>
        /// Type of maintenance
        /// </summary>
        public MaintenanceType Type { get; set; }

        /// <summary>
        /// What was performed
        /// </summary>
        public string WorkDescription { get; set; }

        /// <summary>
        /// Additional comments
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Who performed the maintenance
        /// </summary>
        public string PerformedBy { get; set; }

        /// <summary>
        /// How long the maintenance took
        /// </summary>
        public decimal? Duration { get; set; }

        /// <summary>
        /// Cost of the maintenance
        /// </summary>
        public decimal? RepairPrice { get; set; }

        /// <summary>
        /// Foreign key to the vehicle table
        /// </summary>
        public int IdVehicle { get; set; }

        /// <summary>
        /// Associated vehicle that had this maintenance performed on
        /// </summary>
        public virtual Vehicle Vehicle { get; set; }

        /// <summary>
        /// Foreign key to the users table
        /// </summary>
        [JsonIgnore]
        public int IdUser { get; set; }

        /// <summary>
        /// Owner of this logbook entry
        /// </summary>
        [JsonIgnore]
        public virtual User Owner { get; set; }

        public class MaintenanceEntryValidator : AbstractValidator<MaintenanceEntry>
        {
            public MaintenanceEntryValidator()
            {
                RuleFor(x => x.RepairPrice).GreaterThanOrEqualTo(0).WithMessage("NonNegative")
                    .Unless(x => x.RepairPrice == null);
                RuleFor(x => x.Duration).GreaterThanOrEqualTo(0).WithMessage("NonNegative")
                    .Unless(x => x.Duration == null);
                RuleFor(x => x.MilesPerformed).GreaterThanOrEqualTo(0).WithMessage("NonNegative");
                RuleFor(x => x.WorkDescription).Length(0, 1000).WithMessage("Length1000");
                RuleFor(x => x.Comments).Length(0, 200).WithMessage("Length200");
                RuleFor(x => x.PerformedBy).Length(0, 200).WithMessage("Length200");
            }
        }
    }
}
