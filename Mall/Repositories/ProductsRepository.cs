using Mall.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mall.Repositories
{
    public class ProductsRepository
    {
        private readonly MallDbContext _context;

        public ProductsRepository(MallDbContext context)
        {
            _context = context;
        }

        public Product Get(int? id)
        {
            return _context.Product.Where(x => x.ProductId == id).FirstOrDefault();
        }

        public IQueryable<Product> GetList()
        {
            return _context.Product.AsNoTracking();
        }

        public Product GetWithNavigation(int? id)
        {
            if (id == null) return null;
            //else return _context.Product.Where(x => x.ProductId == id).Include(p => p.StoreIdNavigation).FirstOrDefault();
            else return _context.Product.Include(p => p.StoreIdNavigation).FirstOrDefault(x => x.ProductId == id);
        }

        public void Add(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();
            return;
        }

        public bool Update(Product product)
        {
            if (product == null) return false;
            _context.Update(product);
            _context.SaveChanges();
            return true;
        }

        public void Delete(Product product)
        {
            _context.Product.Remove(product);
            _context.SaveChanges();
        }
    }
}
