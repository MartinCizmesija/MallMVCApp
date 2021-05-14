using System;
using System.ComponentModel.DataAnnotations;
using Filmofile.ViewModels;

namespace Mall.ViewModels
{
    public class StoresViewModel
    {
        
        public int StoreId { get; set; }
        public int RoomId { get; set; }
        
        [Display (Name = "Store name")]
        public string StoreName { get; set; }
        
        [Display (Name = "Store description")]
        public string StoreDescription { get; set; }


        public string DayOfTheWeek { get; set; }
        public DateTime StartingHour { get; set; }
        public DateTime FinishingHour { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
