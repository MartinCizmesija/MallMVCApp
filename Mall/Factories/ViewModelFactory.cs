using Mall.Models;
using Mall.ViewModels;
using System.Collections.Generic;

namespace Mall.Factories
{
    public class ViewModelFactory
    {
        public ViewModelFactory() { 
        }

        public CategoryListViewModel CreateCategoryList (List<Category> categories, PagingInfo pagingInfo)
        {
            return new CategoryListViewModel()
            {
                Categories = categories,
                PagingInfo = pagingInfo
            };
        }

        public ProductViewModel CreateProduct(Product product, List<Category> categories)
        {
            return new ProductViewModel()
            {
                ProductId = product.ProductId,
                Price = product.Price,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                StoreName = product.StoreIdNavigation.StoreName,
                Categories = categories
            };
        }

        public ProductsListViewModel CreateProductList (List<ProductViewModel> products, PagingInfo pagingInfo)
        {
            return new ProductsListViewModel()
            {
                Products = products,
                PagingInfo = pagingInfo
            };
        }

        public RoomsListViewModel CreateRoomList (List<Room> rooms, PagingInfo pagingInfo)
        {
            return new RoomsListViewModel()
            {
                PagingInfo = pagingInfo,
                Rooms = rooms
            };
        }


    }
}

