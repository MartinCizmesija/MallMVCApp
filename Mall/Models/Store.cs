using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mall.Models
{
    public class Store
    {
        public int StoreId { get; set; }

        [Display(Name = "Room number")]
        public int RoomId { get; set; }

        [Display (Name = "Store name")]
        public string StoreName { get; set; }

        [Display (Name = "Store description")]
        public string StoreDescription { get; set; }

        [Display (Name = "Rent debt")]
        public double? RentDebt { get; set; }

        [Display (Name = "Occupied room number")]
        public virtual Room RoomIdNavigation { get; set; }
        public virtual ICollection <User> User { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<WorkingTime> WorkingTime { get; set; }
    }
}
