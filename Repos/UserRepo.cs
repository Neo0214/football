using football.Entities;
using Object_Storage;

namespace football.Repos
{
    public class UserRepo
    {
        private readonly FootballContext _context;
        private readonly ObjectStorageContext _OSS;
        public UserRepo(FootballContext context, ObjectStorageContext objectStorageContext)
        {
            _context = context;
            _OSS = objectStorageContext;
        }

        public User getUserByName(string username)
        {
            return _context.Users.SingleOrDefault(user => user.Name == username);
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
        public byte[] getUserAvatar(int userId)
        {
            // 查出头像图id
            string avatarId = _context.Users.Single(user => user.UserId == userId).AvatarId;

            return _OSS.fetchFile(avatarId);
        }

        public bool addUser(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges() == 0 ? false : true;
        }

    }
}
