using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    public class Airport
    {
        /// <summary>
        /// ICAO or IATA code
        /// </summary>
        public string IDAirport { get; set; }

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

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        /// <summary>
        /// Used for disambiguation when two airports share the same lat/lon
        /// </summary>
        public bool Preferred { get; set; }
    }
}