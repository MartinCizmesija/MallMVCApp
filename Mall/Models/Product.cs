using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mall.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Display(Name = "Store")]
        public int StoreId { get; set; }

        [Display (Name = "Price")]
        public double Price { get; set; }

        [Display(Name = "Product name")]
        [Required]
        public string ProductName { get; set; }

        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        public virtual ICollection <Product_category> Product_Category { get; set; }

        [Display (Name = "Where to find")]
        public virtual Store StoreIdNavigation { get; set; }
    }
}
