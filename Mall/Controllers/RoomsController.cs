using Mall.Factories;
using Mall.Models;
using Mall.Repositories;
using Mall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mall.Controllers
{
    public class RoomsController : Controller
    {
        private readonly RoomRepository _roomRepository;
        private readonly ViewModelFactory _viewModelFactory;
        private readonly AppSettings _appData;
        public RoomsController(IOptionsSnapshot<AppSettings> options,
            RoomRepository roomRepository, ViewModelFactory viewModelFactory)
        {
            _appData = options.Value;
            _roomRepository = roomRepository;
            _viewModelFactory = viewModelFactory;
        }

        //GET: Rooms
        public IActionResult Index(string searchString, int page = 1, int sort = 1, bool ascending = true)
        {
            var query = _roomRepository.GetList();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.RoomId.ToString().Contains(searchString));
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

            var model = _viewModelFactory.CreateRoomList(rooms, pagingInfo);

            return View(model);
        }

        // GET: Rooms/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = _roomRepository.Get(id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Room room)
        {
            room.MallId = 1;
            room.IsAvailable = true;
            if (ModelState.IsValid)
            {
                _roomRepository.Add(room);

                return RedirectToAction(nameof(Index));
            }

            return View(room);
        }

        // GET: Rooms/Edit/5
        public IActionResult Edit(int? id)
        {
            var room = _roomRepository.Get(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Room room)
        {
            if (room == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_roomRepository.Update(room))
                    {
                        TempData[Constants.Message] = "Room Edited";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Delete/5
        public IActionResult Delete(int? id)
        {
            var room = _roomRepository.Get(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var room = _roomRepository.Get(id);
            _roomRepository.Delete(room);
            return RedirectToAction(nameof(Index));
        }
    }
}
