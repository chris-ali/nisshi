using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    /// <summary>
    /// A logged flight
    /// </summary>
    public class LogbookEntry : BaseEntity
    {
        /// <summary>
        /// Associated aircraft that performed the flight in this entry
        /// </summary>
        [JsonPropertyName("associatedAircraft")]
        public virtual Aircraft AssociatedAircraft { get; set; }

        /// <summary>
        /// Date that the flight took place
        /// </summary>
        [JsonPropertyName("flightDate")]
        public DateTime FlightDate { get; set; }

        [JsonPropertyName("numInstrumentApproaches")]
        public int NumInstrumentApproaches { get; set; }

        [JsonPropertyName("numLandings")]
        public int NumLandings { get; set; }

        [JsonPropertyName("numNightLandings")]
        public int NumNightLandings { get; set; }

        [JsonPropertyName("numFullStopLandings")]
        public int NumFullStopLandings { get; set; }

        [JsonPropertyName("crossCountry")]
        public decimal CrossCountry { get; set; }

        [JsonPropertyName("night")]
        public decimal Night { get; set; }

        [JsonPropertyName("IMC")]
        public decimal IMC { get; set; }

        [JsonPropertyName("simulatedInstrument")]
        public decimal SimulatedInstrument { get; set; }

        [JsonPropertyName("dualReceived")]
        public decimal DualReceived { get; set; }

        [JsonPropertyName("dualGiven")]
        public decimal DualGiven { get; set; }

        [JsonPropertyName("PIC")]
        public decimal PIC { get; set; }

        [JsonPropertyName("SIC")]
        public decimal SIC { get; set; }

        [JsonPropertyName("groundSim")]
        public decimal GroundSim { get; set; }

        [JsonPropertyName("hobbsStart")]
        public decimal HobbsStart { get; set; }

        [JsonPropertyName("hobbsEnd")]
        public decimal HobbsEnd { get; set; }

        [JsonPropertyName("totalFlightTimepe")]
        public decimal TotalFlightTime { get; set; }

        [JsonPropertyName("route")]
        public string Route { get; set; }

        [JsonPropertyName("comments")]
        public string Comments { get; set; }

        /// <summary>
        /// Owner of this logbook entry
        /// </summary>
        [JsonIgnore]
        public virtual User Owner { get; set; }
    }
}