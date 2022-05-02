using System.Collections.Generic;

namespace Mall.ViewModels
{
    public class StoresListVeiwModel
    {
        public IEnumerable<StoreViewModel> Stores { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
