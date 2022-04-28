using Mall.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mall.Repositories
{
    public class RoomRepository
    {
        private readonly MallDbContext _context;

        public RoomRepository(MallDbContext context)
        {
            _context = context;
        }

        public Room Get(int? id)
        {
            return _context.Room.Where(x => x.RoomId == id).FirstOrDefault();
        }

        public IQueryable<Room> GetList()
        {
            return _context.Room.AsNoTracking();
        }

        public void Add(Room room)
        {
            _context.Add(room);
            _context.SaveChanges();
            return;
        }

        public bool Update(Room room)
        {
            if (room == null) return false;
            _context.Update(room);
            _context.SaveChanges();
            return true;
        }

        public void Delete(Room room)
        {
            _context.Room.Remove(room);
            _context.SaveChanges();
        }
    }
}
