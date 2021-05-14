namespace Mall.Models
{
    public class Product_category
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public virtual Product ProductIdNavigation { get; set; }
        public virtual Category CategoryIdNavigation { get; set; }
    }
}
