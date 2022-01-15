namespace Nisshi.Models
{
    /// <summary>
    /// Contains the sum of total time for each month of logbook entries
    /// </summary>
    public class TotalTimeByMonth
    {
        public decimal TotalTimeSum { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }
    }

    /// <summary>
    /// Contains the total time for each category class of aircraft
    /// in a logbook entry
    /// </summary>
    public class TotalTimeByCategoryClass
    {
        public decimal TotalTimeSum { get; set; }

        public string CategoryClass { get; set; }
    }
}
