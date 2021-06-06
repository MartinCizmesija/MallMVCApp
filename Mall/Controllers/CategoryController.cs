using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mall.Models;
using Mall.Repositories;
using Mall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mall.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppSettings _appData;

        private CategoryRepository repository = new CategoryRepository();

        public CategoryController(IOptionsSnapshot<AppSettings> options)
        {
            _appData = options.Value;
        }

        // GET: Categories
        public IActionResult Index(string searchString, int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = _appData.PageSize;

            var query = repository.GetList();

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
        public IActionResult Details(int? id)
        {
            var category = repository.Get(id);

            if (category == null) return NotFound();
            else return View(category);
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
                repository.Add(category);
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }


        // GET: Category/Edit/5
        public IActionResult Edit(int? id)
        {
            var category = repository.Get(id);

            if (category == null) return NotFound();
            else return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                if (repository.Update(category))
                {
                    TempData[Constants.Message] = "Category Edited";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                else return NotFound();
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public IActionResult Delete(int? id)
        {
            Category category = repository.Get(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Category category)
        {
            repository.Delete(category);
            return RedirectToAction(nameof(Index));
        }
    }
}
