using System.Linq;
using Mall.Models;
using Microsoft.EntityFrameworkCore;

namespace Mall.Repositories
{
    public class CategoryRepository
    {
        private readonly MallDbContext _context;

        public CategoryRepository()
        {
            _context = new MallDbContext();
        }

        public Category Get (int? id)
        {
            if (id == null) return null;
            else return _context.Category.Find(id);
        }

        public IQueryable<Category> GetList()
        {
            return _context.Category.AsNoTracking();
        }

        public void Add(Category category)
        {
            _context.Add(category);
            _context.SaveChanges();
            return;
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
