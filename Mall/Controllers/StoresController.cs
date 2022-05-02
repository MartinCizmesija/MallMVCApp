using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mall.Factories;
using Mall.Models;
using Mall.Repositories;
using Mall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mall.Controllers
{
    public class StoresController : Controller
    {
        private readonly MallDbContext _contextt;
        private readonly RoomRepository _roomRepository;
        private readonly StoreRepository _storeRepository;
        private readonly ProductRepository _productRepository;
        private readonly ViewModelFactory _viewModelFactoy;
        private readonly AppSettings _appData;

        private IEnumerable<Room> roomList;
        public StoresController(MallDbContext context, IOptionsSnapshot<AppSettings> options,
            StoreRepository storeRepository, ProductRepository productRepository,
            RoomRepository roomRepository, ViewModelFactory viewModelFactory)
        {
            _contextt = context;
            _roomRepository = roomRepository;
            _storeRepository = storeRepository;
            _productRepository = productRepository;
            _viewModelFactoy = viewModelFactory;
            _appData = options.Value;

            roomList = _roomRepository.GetList();
        }

        //GET: Stores
        public IActionResult Index(string searchString, int page = 1, int sort = 1, bool ascending = true)
        {

            var query = _storeRepository.GetList();
            int count = query.Count();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.StoreName.Contains(searchString));
            }

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

            var stores = new List<StoreViewModel>();

            if (page != 0)
            {
                stores = query
                    .Select(m => new StoreViewModel
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

            var model = _viewModelFactoy.CreateStoreList(stores, pagingInfo);

            return View(model);
        }

        // GET: Stores/Details/5
        public IActionResult Details(int? id)
        {
            var store = _storeRepository.Get(id);

            if (store == null)
            {
                return NotFound();
            }

            var products = _productRepository.GetProductsOfStore(store.StoreId);

            var model = _viewModelFactoy.CreateStore(store, products);

            return View(model);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            PrepareDropdownListForRooms();
            return View();
        }

        // POST: Stores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Store store)
        {
            if (ModelState.IsValid)
            {
                var room = _roomRepository.Get(store.RoomId);
                room.IsAvailable = false;

                _roomRepository.Update(room);
                _storeRepository.Add(store);

                return RedirectToAction(nameof(Index));
            }

            return View(store);
        }

        // GET: Stores/Edit/5
        public IActionResult Edit(int? id)
        {
            var store = _storeRepository.Get(id);
            if (store == null)
            {
                return NotFound();
            }

            ViewData["RoomId"] = new SelectList(roomList, "RoomId", "RoomId", store.RoomId);
            return View(store);
        }

        // POST: Stores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Store store)
        {
            if (store == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_storeRepository.Update(store))
                    {
                        TempData[Constants.Message] = "Store Edited";
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

            ViewData["RoomId"] = new SelectList(roomList, "RoomId", "RoomId", store.RoomId);
            return View(store);
        }

        // GET: Stores/Delete/5
        public IActionResult Delete(int? id)
        {
            var store = _storeRepository.GetWithNavigation(id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var store = _storeRepository.Get(id);
            var room = _roomRepository.Get(store.RoomId);
            room.IsAvailable = true;

            _roomRepository.Update(room);
            _storeRepository.Delete(store);
            return RedirectToAction(nameof(Index));
        }

        private void PrepareDropdownListForRooms()
        {
            var rooms = roomList
                .Where(c => c.IsAvailable == true)
                .OrderBy(g => g.RoomId)
                .Select(g => new { g.RoomId })
                .ToList();

            ViewBag.RoomId = new SelectList(rooms, nameof(Room.RoomId), nameof(Room.RoomId));
        }

    }
}
