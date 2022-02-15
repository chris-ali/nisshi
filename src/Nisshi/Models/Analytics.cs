using Nisshi.Infrastructure.Enums;

namespace Nisshi.Models
{
    /// <summary>
    /// Contains the sum of totals for each month of logbook entries
    /// </summary>
    public class TotalsByMonth : SumTotals
    {
        public int Month { get; set; }

        public int Year { get; set; }
    }

    /// <summary>
    /// Contains the totals for each category class of aircraft
    /// in all logbook entries
    /// </summary>
    public class TotalsByCategoryClass : SumTotals
    {
        public string CategoryClass { get; set; }
    }

    /// <summary>
    /// Contains the totals for each type of aircraft
    /// in all logbook entries
    /// </summary>
    public class TotalsByType : SumTotals
    {
        public string Type { get; set; }
    }

    /// <summary>
    /// Contains the totals for each instance (real/simulated) of aircraft
    /// in all logbook entries
    /// </summary>
    public class TotalsByInstanceType : SumTotals
    {
        public string Instance { get; set; }
    }

    /// <summary>
    /// Contains the day/night landings and  for each category class of aircraft
    /// in all logbook entries in the past 90 days
    /// </summary>
    public class LandingsApproaches
    {
        public int DayLandings { get; set; }

        public int NightLandings { get; set; }

        public int Approaches { get; set; }
    }

    /// <summary>
    /// Contains various type of logbook entry time sums
    /// </summary>
    public class SumTotals
    {
        public decimal TotalTimeSum { get; set; } = 0m;

        public decimal NightSum { get; set; } = 0m;

        public decimal MultiSum { get; set; } = 0m;

        public decimal InstrumentSum { get; set; } = 0m;

        public decimal CrossCountrySum { get; set; } = 0m;

        public decimal TurbineSum { get; set; } = 0m;

        public decimal PICSum { get; set; } = 0m;

        public decimal SICSum { get; set; } = 0m;

        public decimal DualGivenSum { get; set; } = 0m;
    }
}
