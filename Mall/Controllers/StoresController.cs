using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Filmofile.ViewModels;
using Mall.Models;
using Mall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mall.Controllers
{
    public class StoresController : Controller
    {
        private readonly MallDbContext _context;
        private readonly AppSettings _appData;
        public StoresController(MallDbContext context, IOptionsSnapshot<AppSettings> options)
        {
            _context = context;
            _appData = options.Value;
        }


        //GET: Stores
        public IActionResult Index(string searchString, int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = _appData.PageSize;

            var query = _context.Store
                        .AsNoTracking();
            
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.StoreName.Contains(searchString));
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

            System.Linq.Expressions.Expression<Func<Store, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = m => m.StoreName;
                    break;
                case 2:
                    orderSelector = m => m.StoreDescription;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }

            var stores = new List<StoresViewModel>();

            if (page != 0)
            {
                stores = query
                    .Select(m => new StoresViewModel
                    {
                        StoreId = m.StoreId,
                        StoreName = m.StoreName,
                        StoreDescription = m.StoreDescription,
                        PagingInfo = pagingInfo
                    })
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();
            }

            return View(stores);
        }

        // GET: Stores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .FirstOrDefaultAsync(m => m.StoreId == id);

            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            PrepareDropdownListForRooms(); 
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(store);
        }

        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", store.RoomId);
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            Store store = await _context.Store.Where(s => s.StoreId == id).FirstOrDefaultAsync();

            if (store == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await TryUpdateModelAsync(store, "", s => s.StoreId, s => s.RoomId, s => s.StoreName,
                        s => s.StoreDescription, s => s.RentDebt))
                    {
                        await _context.SaveChangesAsync();
                        TempData[Constants.Message] = "Store Edited";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.StoreId))
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
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", store.RoomId);
            return View(store);
        }

        // GET: Stores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .Include(s => s.RoomIdNavigation)
                .FirstOrDefaultAsync(m => m.StoreId == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _context.Store.FindAsync(id);
            _context.Store.Remove(store);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
            return _context.Store.Any(e => e.StoreId == id);
        }

        private void PrepareDropdownListForRooms ()
        {
            var rooms = _context.Room
                .Where(c => c.IsAvailable == true)
                .OrderBy(g => g.RoomId)
                .Select(g => new { g.RoomId})
                .ToList();

            ViewBag.RoomId = new SelectList(rooms, nameof(Room.RoomId), nameof(Room.RoomId));
        }

    }
}
