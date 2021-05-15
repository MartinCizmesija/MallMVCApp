using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mall.ViewModels;

namespace Mall.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Display (Name = "Category name")]
        public string CategoryName { get; set; }

        [Display(Name = "Description")]
        public string CategoryDescription { get; set; }

        public virtual ICollection<Product_category> Product_Category { get; set; }
    }
}
