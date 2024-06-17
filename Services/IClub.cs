namespace football.Services
{
    public interface IClub
    {
        int getClubIdByUserId(int userId);
        string getClubLogo(int clubId);
        string getClubNameById(int clubId);
    }
}
