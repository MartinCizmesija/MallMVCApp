using Mall.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mall.Repositories
{
    public class StoreRepository
    {
        private readonly MallDbContext _context;

        public StoreRepository(MallDbContext context)
        {
            _context = context;
        }

        public Store Get(int? id)
        {
            return _context.Store.Where(x => x.StoreId== id).FirstOrDefault();
        }

        public IQueryable<Store> GetList()
        {
            return _context.Store.AsNoTracking();
        }

        public Store GetWithNavigation(int? id)
        {
            if (id == null) return null;
            //else return _context.Product.Where(x => x.ProductId == id).Include(p => p.StoreIdNavigation).FirstOrDefault();
            else return _context.Store.Include(p => p.RoomIdNavigation).FirstOrDefault(x => x.StoreId == id);
        }

        public Store Add(Store store)
        {
            var result = _context.Add(store);
            _context.SaveChanges();
            return result.Entity;
        }

        public bool Update(Store store)
        {
            if (store == null) return false;
            _context.Update(store);
            _context.SaveChanges();
            return true;
        }

        public void Delete(Store store)
        {
            _context.Store.Remove(store);
            _context.SaveChanges();
        }
    }
}
