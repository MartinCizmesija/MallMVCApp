using Mall.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Mall.Repositories
{
    public class CategoryRepository
    {
        private readonly MallDbContext _context;

        public CategoryRepository(MallDbContext context)
        {
            _context = context;
        }

        public Category Get (int? id)
        {
            if (id == null) return null;
            else return _context.Category.Where(x => x.CategoryId == id).FirstOrDefault();
        }

        public IQueryable<Category> GetList()
        {
            return _context.Category.AsNoTracking();
        }

        public List<Category> GetProductCategories(int? id)
        {
            if (id == null) return null;
            return _context.Category.Where(c => c.Product_Category.Any(m => m.ProductId == id)).ToList();
        }

        public Category Add(Category category)
        {
            var result = _context.Add(category);
            _context.SaveChanges();
            return result.Entity;
        }

        public bool Update (Category category)
        {
            if (category == null) return false;
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public void Delete (Category category)
        {
            _context.Category.Remove(category);
           _context.SaveChanges();
        }

    }
}
