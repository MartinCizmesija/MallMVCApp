using System.Collections.Generic;

namespace Mall.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<ProductsViewModel> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
