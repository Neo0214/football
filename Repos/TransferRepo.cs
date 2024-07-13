using football.Entities;
using football.Services;
using Microsoft.EntityFrameworkCore;

namespace football.Repos
{
    public class TransferRepo
    {
        private readonly FootballContext _context;
        public TransferRepo(FootballContext context)
        {
            _context = context;
        }

        public List<Player> getAllPlayer(int clubId)
        {
            List<Player> list = new List<Player>();
            list = _context.Players
                .Include(p => p.Clubs)
                .Where(p => p.Clubs.Any(c => c.ClubId == clubId))
                .ToList();
            return list;
        }

        public bool addTransfer(int from, int to, int value, int playerId)
        {
            Transfer transfer = new Transfer()
            {
                From = from,
                To = to,
                Cost = value,
                PlayerId = playerId
            };
            _context.Transfers.Add(transfer);

            return _context.SaveChanges() > 0;
        }

        public bool movePlayer(int playerId, int toClub, int value)
        {
            Player player = _context.Players.Include(p => p.Clubs).Single(p => p.PlayerId == playerId);
            var club = _context.Clubs.SingleOrDefault(c => c.ClubId == toClub);
            player.Clubs.Remove(player.Clubs.First());
            player.Clubs.Add(club);
            player.Value = value;
            _context.Update(player);

            return _context.SaveChanges() > 0;
        }

        public List<Transfer> getAllTransfer(int playerId)
        {
            return _context.Transfers.Where(t => t.PlayerId == playerId).ToList();
        }
    }
}
