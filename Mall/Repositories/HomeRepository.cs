using Mall.Models;
using System.Linq;

namespace Mall.Repositories
{
    public class HomeRepository
    {
        private readonly MallDbContext _context;

        public HomeRepository(MallDbContext context)
        {
            _context = context;
        }

        public MallCenter Get(int? id)
        {
            if (id == null) return null;
            else return  _context.Mall.Where(s => s.MallId == id).FirstOrDefault();
        }

        public MallCenter Add(MallCenter mall)
        {
            var result = _context.Add(mall);
            _context.SaveChanges();
            return result.Entity;
        }

    }
}
