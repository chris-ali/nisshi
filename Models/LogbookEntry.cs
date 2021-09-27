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
        public DateTime FlightDate { get; set; }

        public int NumInstrumentApproaches { get; set; }

        public int NumLandings { get; set; }

        public int NumNightLandings { get; set; }

        public int NumFullStopLandings { get; set; }

        public decimal CrossCountry { get; set; }

        public decimal Night { get; set; }

        public decimal IMC { get; set; }

        public decimal SimulatedInstrument { get; set; }

        public decimal DualReceived { get; set; }

        public decimal DualGiven { get; set; }

        public decimal PIC { get; set; }

        public decimal SIC { get; set; }

        public decimal GroundSim { get; set; }

        public decimal HobbsStart { get; set; }

        public decimal HobbsEnd { get; set; }

        public decimal TotalFlightTime { get; set; }

        public string Route { get; set; }

        public string Comments { get; set; }

        /// <summary>
        /// Owner of this logbook entry
        /// </summary>
        [JsonIgnore]
        public virtual User Owner { get; set; }
    }
}