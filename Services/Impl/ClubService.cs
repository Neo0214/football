using football.Entities;
using football.Models;
using football.Repos;
using Object_Storage;

namespace football.Services.Impl
{
    public class ClubService : IClub
    {
        private readonly ClubRepo _repo;

        public ClubService(ClubRepo repo)
        {
            _repo = repo;
        }
        public int getClubIdByUserId(int userId)
        {
            return _repo.getClubIdByUserId(userId);
        }
        public byte[] getClubLogo(int clubId)
        {
            return _repo.getClubLogo(clubId);
        }
        public string getClubNameById(int clubId)
        {
            return _repo.getClubNameById(clubId);
        }

        public AllClubDTO getAllClubs(int curClub)
        {
            List<Club> clubs = _repo.getAllClub();
            AllClubDTO output = new AllClubDTO();
            foreach (Club club in clubs)
            {
                if (club.ClubId != curClub)
                {
                    output.addOne(club.ClubId, club.ClubName);
                }
            }
            return output;
        }

        public AllClubScoreDTO getAllClubsWithScore(int curClub)
        {
            List<Club> clubs = _repo.getAllClub();
            AllClubScoreDTO output = new AllClubScoreDTO();
            foreach (Club club in clubs)
            {
                if (club.ClubId != curClub)
                {
                    output.addOne(club.ClubId, club.ClubName, club.Score);
                }
            }
            return output;
        }
    }
}
