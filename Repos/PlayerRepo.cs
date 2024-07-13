using football.Entities;
using Microsoft.EntityFrameworkCore;
using Object_Storage;
using football.Models;

namespace football.Repos
{
    public class PlayerRepo
    {
        public readonly FootballContext _context;
        public readonly ObjectStorageContext _OSS;
        public PlayerRepo(FootballContext context, ObjectStorageContext objectStorageContext)
        {
            _context = context;
            _OSS = objectStorageContext;
        }
        public List<Player> getAll()
        {
            List<Player> list = new List<Player>();
            list = _context.Players.Include(p => p.Clubs).Include(p => p.Nations).ToList();
            return list;
        }
        public byte[] getPlayerPic(int playerId)
        {
            Player player = _context.Players.Single(p => p.PlayerId == playerId);
            if (player == null)
            {
                // 如果找不到玩家，可以返回空数组或者抛出异常，具体根据需求决定
                throw new InvalidOperationException($"No player found with ID {playerId}");
            }
            string picId = player.AvatarId;
            // 获取pic
            return _OSS.fetchFile(picId);
        }
        public int addPlayer(AddPlayerDTO playerDTO)
        {
            DateTime startTime = DateTime.Parse(playerDTO.expire_date, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateOnly date = DateOnly.FromDateTime(startTime);
            // 查出对应国籍
            var nation = _context.Nations.FirstOrDefault(nameof => nameof.NationName == playerDTO.nation);
            var club = _context.Clubs.FirstOrDefault(idof => idof.ClubId == playerDTO.club_id);
            // 添加球员
            Player player = new Player
            {
                Name = playerDTO.name,
                Position = playerDTO.position,
                Value = playerDTO.value,
                ExpireDate = date,
                Score = playerDTO.score,
                Shot = playerDTO.shot,
                Pass = playerDTO.pass,
                Dribble = playerDTO.dribble,
                Defence = playerDTO.defence,
                Vel = playerDTO.vel,
                Energy = playerDTO.energy,
                Nations = new List<Nation> { nation },
                Clubs = new List<Club> { club },

            };
            _context.Players.Add(player);

            // 尝试保存变更
            int rowsAffected = _context.SaveChanges();

            // 检查保存结果
            if (rowsAffected > 0)
            {

                // 返回新加一行的id值

                return player.PlayerId;
            }
            else
            {
                // 添加失败，返回一个标识
                return -1; // 或者其他适当的标识
            }
        }
        public bool addPlayerPic(byte[] bytes, int playerId)
        {
            string picId = _OSS.store(bytes);
            var player = _context.Players.FirstOrDefault(p => p.PlayerId == playerId);
            player.AvatarId = picId;
            return _context.SaveChanges() != 0 ? true : false;
        }
        public Player getPlayer(int playerId)
        {
            return _context.Players.Include(p => p.Clubs).Include(p => p.Nations).Single(p => p.PlayerId == playerId);
        }

        public List<Player> getPlayerWithClub(int clubId)
        {
            return _context.Players.Include(p => p.Clubs).Where(p => p.Clubs.Where(c => c.ClubId == clubId).Any()).ToList();
        }
    }
}
