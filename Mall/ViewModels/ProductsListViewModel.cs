using System.Collections.Generic;

namespace Mall.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
