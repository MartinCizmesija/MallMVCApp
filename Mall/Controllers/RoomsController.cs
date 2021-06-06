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
    public class RoomsController : Controller
    {
        private readonly MallDbContext _context;
        private readonly AppSettings _appData;
        public RoomsController(MallDbContext context, IOptionsSnapshot<AppSettings> options)
        {
            _context = context;
            _appData = options.Value;
        }

        //GET: Stores
        public IActionResult Index(string searchString, int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = _appData.PageSize;

            var query = _context.Room
                        .AsNoTracking();

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.RoomId.ToString().Contains(searchString));
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

            System.Linq.Expressions.Expression<Func<Room, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = m => m.RoomId;
                    break;
                case 2:
                    orderSelector = m => m.Rent;
                    break;
                case 3:
                    orderSelector = m => m.IsAvailable;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }

            var rooms = new List<Room>();

            if (page != 0)
            {
                rooms = query
                    .Select(m => new Room
                    {
                        RoomId = m.RoomId,
                        MallId = m.MallId,
                        Rent = m.Rent,
                        IsAvailable = m.IsAvailable
                    })
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();
            }

            var model = new RoomsListViewModel
            {
                Rooms = rooms,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        // GET: Stores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .FirstOrDefaultAsync(m => m.RoomId == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Room room)
        {
            room.MallId = 1;
            room.IsAvailable = true;
            if (ModelState.IsValid)
            {
                _context.Add(room);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(room);
        }

        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            Room room = await _context.Room.Where(s => s.RoomId == id).FirstOrDefaultAsync();

            if (room == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await TryUpdateModelAsync(room, "", s => s.RoomId, s => s.MallId, s => s.Rent,
                        s => s.IsAvailable))
                    {
                        await _context.SaveChangesAsync();
                        TempData[Constants.Message] = "Room Edited";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
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
            return View(room);
        }

        // GET: Stores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Room.FindAsync(id);
            _context.Room.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Room.Any(e => e.RoomId == id);
        }

    }
}
