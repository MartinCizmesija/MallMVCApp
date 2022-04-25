using Mall.Factories;
using Mall.Models;
using Mall.Repositories;
using Mall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mall.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductRepository _productsRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly StoreRepository _storesRepo;
        private readonly ViewModelFactory _viewModelsFactory;
        private readonly ModelFactory _modelFactory;
        private readonly AppSettings _appData;

        private readonly IEnumerable<Store> storeList;

        public ProductsController(IOptionsSnapshot<AppSettings> options,
            ProductRepository productsRepo, CategoryRepository categoryRepo,
            StoreRepository storesRepo, ViewModelFactory viewModelsFactory,
            ModelFactory modelFactory)
        {
            _productsRepo = productsRepo;
            _categoryRepo = categoryRepo;
            _storesRepo = storesRepo;
            _viewModelsFactory = viewModelsFactory;
            _modelFactory = modelFactory;
            _appData = options.Value;

            storeList = _storesRepo.GetList();
        }

        // GET: Products
        public IActionResult Index(string searchString, int page = 1, int sort = 1, bool ascending = true)
        {
            var query = _productsRepo.GetList();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.ProductName.Contains(searchString));
            }

            int count = query.Count();
            int pagesize = _appData.PageSize;

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };
            if (page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort = sort, ascending = ascending });
            }

            System.Linq.Expressions.Expression<Func<Product, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = m => m.ProductName;
                    break;
                case 2:
                    orderSelector = m => m.Price;
                    break;
                case 3:
                    orderSelector = m => m.ProductDescription;
                    break;
                case 4:
                    orderSelector = m => m.StoreIdNavigation.StoreName;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }

            var products = new List<ProductViewModel>();

            if (page != 0)
            {
                products = query
                    .Select(m => new ProductViewModel
                    {
                        ProductId = m.ProductId,
                        Price = m.Price,
                        ProductName = m.ProductName,
                        ProductDescription = m.ProductDescription,
                        StoreName = m.StoreIdNavigation.StoreName,
                        PagingInfo = pagingInfo
                    })
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();
            }

            var model = _viewModelsFactory.CreateProductList(products, pagingInfo);
            return View(model);
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productsRepo.GetWithNavigation(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = _categoryRepo.GetProductCategories(id);

            var model = _viewModelsFactory.CreateProduct(product, categories);

            return View(model);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            PrepareDropdownListForCategories();
            ViewData["StoreId"] = new SelectList(storeList, "StoreId", "StoreName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductId,StoreId,Price,ProductName,ProductDescription")] ProductViewModel productModel)
        {
            var product = _modelFactory.CreateProduct(productModel);

            if (ModelState.IsValid)
            {
                _productsRepo.Add(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(storeList, "StoreId", "StoreName", product.StoreId);
            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)
        {
            var product = _productsRepo.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["StoreId"] = new SelectList(storeList, "StoreId", "StoreName", product.StoreId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductId,StoreId,Price,ProductName,ProductDescription")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            var productUpdated = _productsRepo.Update(product);
            if (!productUpdated)
            {
                return NotFound();
            }

            TempData[Constants.Message] = "Product Edited";
            TempData[Constants.ErrorOccurred] = false;
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            var product = _productsRepo.GetWithNavigation(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _productsRepo.Get(id);
            _productsRepo.Delete(product);
            return RedirectToAction(nameof(Index));
        }

        private void PrepareDropdownListForCategories()
        {
            var categories = _categoryRepo.GetList();

            ViewBag.Categories = new SelectList(categories, nameof(Category.CategoryId), nameof(Category.CategoryName));
        }
    }
}
