using System;

namespace Mall.Models
{
    public class WorkingTime
    {
        public int WorkingTimeId { get; set; }
        public int StoreId { get; set; }
        public string DayOfTheWeek { get; set; }
        public DateTime StartingHour { get; set; }
        public DateTime FinishingHour { get; set; }
        public bool MallWorkingTime { get; set; }

        public virtual MallCenter MallNavigation { get; set; }
        public virtual Store StoreIdNavigation { get; set; }
    }
}
