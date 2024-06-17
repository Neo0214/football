using football.ExLibs;
using football.Repos;

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
        public string getClubLogo(int clubId)
        {
            FileHandler fileHandler = new FileHandler();
            return fileHandler.getClubPicById(clubId);
        }
        public string getClubNameById(int clubId)
        {
            return _repo.getClubNameById(clubId);
        }
    }
}
