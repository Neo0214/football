using football.Repos;
using football.Models;
using football.Services;
using football.Entities;

namespace football.Services.Impl
{
    public class TransferService : ITransfer
    {
        private readonly TransferRepo _repo;
        private readonly ClubRepo _clubRepo;
        public TransferService(TransferRepo repo, ClubRepo clubRepo)
        {
            _repo = repo;
            _clubRepo = clubRepo;
        }

        public PlayerInfoWithPosition getPlayerByPosition(int clubId)
        {
            PlayerInfoWithPosition output = new PlayerInfoWithPosition();
            List<Player> players = _repo.getAllPlayer(clubId);
            foreach (Player player in players)
            {
                output.addOne(player.Position, player.PlayerId, player.Name);
            }
            return output;

        }

        public string transferPlayer(TransferPlayerDTO transferPlayerDTO)
        {
            if (_repo.addTransfer(transferPlayerDTO.oldClubId, transferPlayerDTO.clubId, transferPlayerDTO.fee, transferPlayerDTO.playerId)
                && _repo.movePlayer(transferPlayerDTO.playerId, transferPlayerDTO.clubId, transferPlayerDTO.fee))
                return "success";
            return "fail";
        }

        public TransferAllDTO getAll(int playerId)
        {
            List<Transfer> transfers = _repo.getAllTransfer(playerId);
            TransferAllDTO output = new TransferAllDTO();
            foreach (Transfer transfer in transfers)
            {
                output.addOne(new TransferLine(_clubRepo.getClubNameById(transfer.From), _clubRepo.getClubNameById(transfer.To), transfer.Cost));
            }
            return output;
        }
    }
}
