using Mall.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Mall.Repositories
{
    public class ProductRepository
    {
        private readonly MallDbContext _context;

        public ProductRepository(MallDbContext context)
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

        public List<Product> GetProductsOfStore(int? storeId)
        {
            return _context.Product.Where(m => m.StoreId == storeId).ToList();
        }

        public Product GetWithNavigation(int? id)
        {
            if (id == null) return null;
            //else return _context.Product.Where(x => x.ProductId == id).Include(p => p.StoreIdNavigation).FirstOrDefault();
            else return _context.Product.Include(p => p.StoreIdNavigation).FirstOrDefault(x => x.ProductId == id);
        }

        public Product Add(Product product)
        {
            var result = _context.Add(product);
            _context.SaveChanges();
            return result.Entity;
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
