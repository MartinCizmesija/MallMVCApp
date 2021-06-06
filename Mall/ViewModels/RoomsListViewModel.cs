using System.Collections.Generic;
using Mall.Models;

namespace Mall.ViewModels
{
    public class RoomsListViewModel
    {
        public List<Room> Rooms { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
