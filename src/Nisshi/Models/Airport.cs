using System;

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
    }
}