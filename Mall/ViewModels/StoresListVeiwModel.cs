using System.Collections.Generic;

namespace Mall.ViewModels
{
    public class StoresListVeiwModel
    {
        public IEnumerable<StoresViewModel> Stores { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
