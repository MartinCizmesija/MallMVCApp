using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mall.Models;
using Mall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mall.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MallDbContext _context;
        private readonly AppSettings _appData;

        public ProductsController(MallDbContext context, IOptionsSnapshot<AppSettings> options)
        {
            _context = context;
            _appData = options.Value;
        }

        // GET: Products
        public IActionResult Index(string searchString, int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = _appData.PageSize;

            var query = _context.Product
                        .AsNoTracking();

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.ProductName.Contains(searchString));
            }

            int count = query.Count();
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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.StoreIdNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductsViewModel
            {
                ProductId = product.ProductId,
                Price = product.Price,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                StoreName = product.StoreIdNavigation.StoreName,
                Categories = _context.Category.Where(c => c.Product_Category.Any(m => m.ProductId == id)).ToList()
            };

            return View(model);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["StoreId"] = new SelectList(_context.Store, "StoreId", "StoreName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,StoreId,Price,ProductName,ProductDescription")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(_context.Store, "StoreId", "StoreName", product.StoreId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,StoreId,Price,ProductName,ProductDescription")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(_context.Store, "StoreId", "StoreName", product.StoreId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.StoreIdNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
