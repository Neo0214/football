using football.Models;
using football.Entities;

namespace football.Services
{
    public interface IPlayer
    {
        TransferDTO getAllPlayerTransfer();
        DataDTO getAllPlayerData();
        byte[] getPlayerPic(int playerId);
        UploadDTO addPlayer(AddPlayerDTO playerDTO);
        PicOkDTO uploadPlayerPic(IFormFile file, int playerId);
        PlayerDetailDTO getPlayerDetail(int playerId);
        MyPlayerDTO getMyPlayer(int clubId);
    }
}
