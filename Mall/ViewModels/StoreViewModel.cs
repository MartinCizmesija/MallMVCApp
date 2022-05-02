using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mall.Models;

namespace Mall.ViewModels
{
    public class StoreViewModel
    {
        public int StoreId { get; set; }
        public int RoomId { get; set; }
        
        [Display (Name = "Store name")]
        public string StoreName { get; set; }
        
        [Display (Name = "Store description")]
        public string StoreDescription { get; set; }
        
        [Display(Name = "Rent debt")]
        public double? RentDebt { get; set; }

        [Display(Name = "Occupied room number")]
        public virtual Room RoomIdNavigation { get; set; }
        
        public List <Product> Products { get; set; }

        public string DayOfTheWeek { get; set; }
        public DateTime StartingHour { get; set; }
        public DateTime FinishingHour { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
