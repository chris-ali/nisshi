using System;
using FluentValidation;

namespace Nisshi.Models
{
    /// <summary>
    /// Representation of an airport
    /// </summary>
    public class Airport
    {
        /// <summary>
        /// ICAO or IATA code
        /// </summary>
        public string AirportCode { get; set; }

        /// <summary>
        /// Friendly name of airport
        /// </summary>
        public string FacilityName { get; set; }

        /// <summary>
        /// Type of facility (not just airports)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Username of creator; empty for non-user generated
        /// </summary>
        public string SourceUserName { get; set; }

        /// <summary>
        /// Facility latitude position [deg, N positive]
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Facility longitude position [deg, E positive]
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Used for disambiguation when two airports share the same lat/lon
        /// </summary>
        public bool Preferred { get; set; }

        /// <summary>
        /// Date entity was first created
        /// </summary>
        /// <value></value>
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Date entity was last updated
        /// </summary>
        public DateTime? DateUpdated { get; set; }

        public class AirportValidator : AbstractValidator<Airport>
        {
            public AirportValidator()
            {
                RuleFor(x => x.AirportCode).NotEmpty().WithMessage("NotEmpty")
                    .Length(3, 4).WithMessage("AirportCode");
                RuleFor(x => x.FacilityName).NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(100).WithMessage("Length100");
                RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude");
                RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude");
            }
        }
    }
}
