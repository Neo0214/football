using football.Entities;
using pwd;
using football.Models;
using football.Repos;
using Object_Storage;

namespace football.Services.Impl
{
    public class UserService : IUser
    {
        private readonly UserRepo _userRepo;
        private readonly ClubRepo _clubRepo;

        public UserService(UserRepo userRepo, ClubRepo clubRepo)
        {
            _userRepo = userRepo;
            _clubRepo = clubRepo;
        }

        public User login(string username, string password)
        {
            User user = _userRepo.getUserByName(username);
            string encodedPwd = PwdEncoder.EncryptString(password);
            if (user.Pwd == encodedPwd)
            {
                return user;
            }
            return null;
        }
        public UserInfoDTO getUserInfo(int userId)
        {
            User user = _userRepo.getUserById(userId); // 取出对应User
            if (user != null)
            {
                int clubId = _clubRepo.getClubIdByUserId(userId);
                return new UserInfoDTO(user.Name, user.TeleNumber, user.Email, user.Address, clubId);
            }
            return null;
        }
        public byte[] getUserAvatar(int userId)
        {
            return _userRepo.getUserAvatar(userId); // 取出对应User的头像（二进制数据）(byte[]
        }
        public UserHistoryListDTO getUserHistory(int userId)
        {
            List<Info> infos = _userRepo.getHistoryById(userId);
            UserHistoryListDTO output = new UserHistoryListDTO();
            for (int i = infos.Count() - 1; i >= 0; i--)
            {
                Info info = infos[i];
                output.add(new History(info.Status, info.Addition, info.StartYear, info.EndYear));
            }
            return output;
        }
        public string addHistory(int userId, AddHistoryDTO addHistoryDTO)
        {
            DateTime startTime = DateTime.Parse(addHistoryDTO.startYear, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTime endTime = DateTime.Parse(addHistoryDTO.endYear, null, System.Globalization.DateTimeStyles.RoundtripKind);
            // 从 DateTime 中提取 DateOnly
            DateOnly dateOnly = DateOnly.FromDateTime(startTime);
            DateOnly dateOnly2 = DateOnly.FromDateTime(endTime);
            Info info = new Info
            {
                UserId = userId,
                StartYear = dateOnly,
                EndYear = dateOnly2,
                Status = addHistoryDTO.title,
                Addition = addHistoryDTO.content
            };
            return _userRepo.AddHistoryAsync(info) ? "success" : "failed";

        }

        public string checkName(string newName)
        {
            if (_userRepo.getUserByName(newName) != null)
            {
                return "failed";
            }
            return "success";
        }

        public string register(RegisterDTO registerDTO)
        {
            string pwd = PwdEncoder.EncryptString(registerDTO.password);
            User user = new User
            {
                Name = registerDTO.name,
                Email = registerDTO.email,
                TeleNumber = registerDTO.teleNumber,
                Address = registerDTO.address,
                Pwd = pwd
            };
            return _userRepo.addUser(user) ? "success" : "failed";
        }
    }
}
