using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mall.Models;
using Mall.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mall.Controllers
{
    public class CategoryController : Controller
    {
        private readonly MallDbContext _context;
        private readonly AppSettings _appData;

        public CategoryController(MallDbContext context, IOptionsSnapshot<AppSettings> options)
        {
            _context = context;
            _appData = options.Value;
        }

        // GET: Categories
        public IActionResult Index(string searchString, int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = _appData.PageSize;

            var query = _context.Category
                        .AsNoTracking();

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.CategoryName.Contains(searchString));
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

            System.Linq.Expressions.Expression<Func<Category, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = m => m.CategoryName;
                    break;
                case 2:
                    orderSelector = m => m.CategoryDescription;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }

            var categories = new List<Category>();

            if (page != 0)
            {
                categories = query
                    .Select(m => new Category
                    {
                        CategoryId = m.CategoryId,
                        CategoryName = m.CategoryName,
                        CategoryDescription = m.CategoryDescription
                    })
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();
            }

            var model = new CategoryListViewModel
            {
                Categories = categories,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }


        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await _context.Category.Where(s => s.CategoryId == id).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await TryUpdateModelAsync(category, "", s => s.CategoryId, s => s.CategoryName, s => s.CategoryDescription))
                    {
                        await _context.SaveChangesAsync();
                        TempData[Constants.Message] = "Category Edited";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }

    }
}
