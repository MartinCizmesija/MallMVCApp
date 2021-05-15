using System.Collections.Generic;

namespace Mall.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public virtual ICollection<Product_category> Product_Category { get; set; }
    }
}
