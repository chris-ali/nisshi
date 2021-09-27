using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    public class Airport
    {
        /// <summary>
        /// ICAO or IATA code
        /// </summary>
        [JsonPropertyName("idAirport")]
        public string IDAirport { get; set; }

        /// <summary>
        /// Friendly name of airport
        /// </summary>
        /// <value></value>
        [JsonPropertyName("facilityName")]
        public string FacilityName { get; set; }

        /// <summary>
        /// Type of facility (not just airports)
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Username of creator; empty for non-user generated
        /// </summary>
        [JsonPropertyName("sourceUserName")]
        public string SourceUserName { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// Used for disambiguation when two airports share the same lat/lon
        /// </summary>
        [JsonPropertyName("preferred")]
        public bool Preferred { get; set; }
    }
}