using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mall.Models;

namespace Mall.ViewModels
{
    public class CategoryListViewModel
    {
        public List<Category> Categories { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
