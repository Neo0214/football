using football.Entities;
using Microsoft.EntityFrameworkCore;

namespace football.Repos
{
    public class PlayerRepo
    {
        public readonly FootballContext _context;
        public PlayerRepo(FootballContext context)
        {
            _context = context;
        }
        public List<Player> getAll()
        {
            List<Player> list = new List<Player>();
            list = _context.Players.Include(p => p.Clubs).Include(p => p.Nations).ToList();
            return list;
        }
    }
}
