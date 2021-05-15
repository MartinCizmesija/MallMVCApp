using System.Collections.Generic;

namespace Mall.Models
{
    public class MallCenter
    {
        public int MallId { get; set; }
        public string MallName { get; set; }
        public string MallDescription { get; set; }

        public virtual ICollection <Room> Room { get; set; }
        public virtual ICollection <User> User { get; set; }
        public virtual ICollection<WorkingTime> WorkingTime { get; set; }

    }
}
