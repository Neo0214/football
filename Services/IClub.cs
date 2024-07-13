using football.Models;

namespace football.Services
{
    public interface IClub
    {
        int getClubIdByUserId(int userId);
        byte[] getClubLogo(int clubId);
        string getClubNameById(int clubId);
        AllClubDTO getAllClubs(int curClub);
        AllClubScoreDTO getAllClubsWithScore(int curClub);
        
    }
}
