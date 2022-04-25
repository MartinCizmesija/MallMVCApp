using Mall.Factories;
using Mall.Models;
using Mall.Repositories;
using Mall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mall.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppSettings _appData;
        private readonly CategoryRepository _repository;
        private readonly ViewModelFactory _viewModelsFactory;
        
        public CategoryController(IOptionsSnapshot<AppSettings> options,
            CategoryRepository repository, ViewModelFactory viewModelsFactory)
        {
            _repository = repository;
            _appData = options.Value;
            _viewModelsFactory = viewModelsFactory;
        }

        // GET: Categories
        public IActionResult Index(string searchString, int page = 1, int sort = 1, bool ascending = true)
        {
            var query = _repository.GetList();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.CategoryName.Contains(searchString));
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

            var model = _viewModelsFactory.CreateCategoryList(categories, pagingInfo);

            return View(model);
        }

        // GET: Category/Details/5
        public IActionResult Details(int? id)
        {
            var category = _repository.Get(id);

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
                _repository.Add(category);
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }


        // GET: Category/Edit/5
        public IActionResult Edit(int? id)
        {
            var category = _repository.Get(id);

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
                if (_repository.Update(category))
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
            Category category = _repository.Get(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Category category)
        {
            _repository.Delete(category);
            return RedirectToAction(nameof(Index));
        }
    }
}
