using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mall.Models;

namespace Mall.ViewModels
{
    public class ProductsViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Store")]
        public int StoreId { get; set; }

        [Display(Name = "Price")]
        public double Price { get; set; }

        [Display(Name = "Product name")]
        [Required]
        public string ProductName { get; set; }

        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        [Display(Name = "Categories")]
        public List<Category> Categories { get; set; }

        [Display(Name = "Where to find")]
        public string StoreName { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
