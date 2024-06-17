using football.Entities;
using football.Models;

namespace football.Services
{
    public interface IUser
    {
        User login(string username, string password);
        UserInfoDTO getUserInfo(int userId);
        string getUserAvatar(int userId);
        UserHistoryListDTO getUserHistory(int userId);
        string addHistory(int userId, AddHistoryDTO addHistoryDTO);
    }
}
