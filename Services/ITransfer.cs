using football.Models;
namespace football.Services
{
    public interface ITransfer
    {
        PlayerInfoWithPosition getPlayerByPosition(int clubId);
        string transferPlayer(TransferPlayerDTO transferPlayerDTO);
        TransferAllDTO getAll(int playerId);
    }
}
