using System;

namespace World_yachts.Model.Logic
{
    public class Date
    {
        public int Day { get; set; }

        public int Week { get; set; }

        public int Month { get; set; }

        public int Quarter { get; set; }

        public int Year { get; set; }

        public Date(DateTime date)
        {
            Day = date.Day;
            Week = GetWeekNumber(date);
            Month = date.Month;
            Quarter = (date.Month / 3) % 3 == 0 ? date.Month / 3 : date.Month / 3 + 1;
            Year = date.Year;
        }

        private int GetWeekNumber(DateTime date)
        {
            return (date.DayOfYear + (int)new DateTime(date.Year, 1, 1).DayOfWeek) / 7 + 1;
        }
    }
}
