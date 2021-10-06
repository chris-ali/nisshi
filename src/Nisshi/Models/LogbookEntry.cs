using System;
using System.Text.Json.Serialization;
using Nisshi.Models.Users;

namespace Nisshi.Models
{
    /// <summary>
    /// A logged flight
    /// </summary>
    public class LogbookEntry : BaseEntity
    {
        /// <summary>
        /// Date that the flight took place
        /// </summary>
        public DateTime FlightDate { get; set; }

        public int NumInstrumentApproaches { get; set; }

        public int NumLandings { get; set; }

        public int NumNightLandings { get; set; }

        public int NumFullStopLandings { get; set; }

        public decimal CrossCountry { get; set; }

        public decimal MultiEngine { get; set; }

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
        /// Foreign key to the aircraft table
        /// </summary>
        public int IdAircraft { get; set; }

        /// <summary>
        /// Associated aircraft that performed the flight in this entry
        /// </summary>
        public virtual Aircraft Aircraft { get; set; }

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
    }
}