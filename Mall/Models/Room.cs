using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mall.Models
{
    public class Room
    {
        [Display (Name = "Room number")]
        public int RoomId { get; set; }
        public int MallId { get; set; }
        public double Rent { get; set; }
        public bool IsAvailable { get; set; }

        public virtual MallCenter MallIdNavigation {get; set;}
        public virtual ICollection <Store> Store { get; set; }
    }
}
