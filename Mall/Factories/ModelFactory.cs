using Mall.Models;
using Mall.ViewModels;

namespace Mall.Factories
{
    public class ModelFactory
    {
        public Product CreateProduct(ProductViewModel viewModel)
        {
            return new Product
            {
                ProductId = viewModel.ProductId,
                StoreId = viewModel.StoreId,
                Price = viewModel.Price,
                ProductName = viewModel.ProductName,
                ProductDescription = viewModel.ProductDescription
            };
        }

    }
}
