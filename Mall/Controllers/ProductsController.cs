using Mall.Models;
using Mall.Repositories;
using Mall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mall.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductsRepository _productsRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly AppSettings _appData;

        //temp
        private readonly MallDbContext _context;

        public ProductsController(IOptionsSnapshot<AppSettings> options,
            ProductsRepository productsRepo, CategoryRepository categoryRepo,
            MallDbContext context)
        {
            _context = context;
            _productsRepo = productsRepo;
            _categoryRepo = categoryRepo;
            _appData = options.Value;
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

            var products = new List<ProductsViewModel>();

            if (page != 0)
            {
                products = query
                    .Select(m => new ProductsViewModel
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

            var model = new ProductsListViewModel
            {
                Products = products,
                PagingInfo = pagingInfo
            };

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
            var model = new ProductsViewModel
            {
                ProductId = product.ProductId,
                Price = product.Price,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                StoreName = product.StoreIdNavigation.StoreName,
                Categories = categories
            };

            return View(model);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            PrepareDropdownListForCategories();
            ViewData["StoreId"] = new SelectList(_context.Store, "StoreId", "StoreName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,StoreId,Price,ProductName,ProductDescription")] ProductsViewModel productModel)
        {
            Product product = new Product
            {
                ProductId = productModel.ProductId,
                StoreId = productModel.StoreId,
                Price = productModel.Price,
                ProductName = productModel.ProductName,
                ProductDescription = productModel.ProductDescription
            };

            if (ModelState.IsValid)
            {
                _productsRepo.Add(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(_context.Store, "StoreId", "StoreName", product.StoreId);
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
            ViewData["StoreId"] = new SelectList(_context.Store, "StoreId", "StoreName", product.StoreId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            ViewData["StoreId"] = new SelectList(_context.Store, "StoreId", "StoreName", product.StoreId);
            return View(product);
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
