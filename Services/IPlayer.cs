using football.Models;

namespace football.Services
{
    public interface IPlayer
    {
        TransferDTO getAllPlayerTransfer();
        DataDTO getAllPlayerData();
        string getPlayerPic(int playerId);
    }
}
