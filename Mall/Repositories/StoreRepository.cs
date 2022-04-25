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

        public IQueryable<Store> GetList()
        {
            return _context.Store.AsNoTracking();
        }
    }
}
