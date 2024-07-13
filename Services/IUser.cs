using football.Entities;
using football.Models;

namespace football.Services
{
    public interface IUser
    {
        User login(string username, string password);
        UserInfoDTO getUserInfo(int userId);
        byte[] getUserAvatar(int userId);
        UserHistoryListDTO getUserHistory(int userId);
        string addHistory(int userId, AddHistoryDTO addHistoryDTO);
        string checkName(string newName);
        string register(RegisterDTO registerDTO);
    }
}
