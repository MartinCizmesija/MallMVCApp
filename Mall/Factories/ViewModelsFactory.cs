using Mall.Models;
using Mall.ViewModels;
using System.Collections.Generic;

namespace Mall.Factories
{
    public class ViewModelsFactory
    {
        public ViewModelsFactory() { 
        }

        public CategoryListViewModel CreateCategoryList (List<Category> categories, PagingInfo pagingInfo)
        {
            return new CategoryListViewModel()
            {
                Categories = categories,
                PagingInfo = pagingInfo
            };
        }

    }
}

