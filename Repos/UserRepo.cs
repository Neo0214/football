using football.Entities;

namespace football.Repos
{
    public class UserRepo
    {
        private readonly FootballContext _context;
        public UserRepo(FootballContext context)
        {
            _context = context;
        }

        public User getUserByName(string username)
        {
            return _context.Users.Single(user => user.Name == username);
        }
        public User getUserById(int userId)
        {
            return _context.Users.Single(user => user.UserId == userId);
        }
        public List<Info> getHistoryById(int userId)
        {
            return _context.Infos.Where(info => info.UserId == userId).ToList();
        }
        public bool AddHistoryAsync(Info info)
        {
            _context.Infos.Add(info);
            return _context.SaveChanges() == 0 ? false : true;
        }

    }
}
