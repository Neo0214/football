using football.Entities;
using Microsoft.AspNetCore.Mvc;

namespace football.ExLibs
{
    public class FileHandler
    {
        private string root = "D:/football/";

        public string getClubPicById(int id)
        {
            string path = root + "club-pic/" + id + ".png";
            return path;
        }
        public string getUserAvatarById(int id)
        {
            string path = root + "user-avatar/" + id + ".jpg";
            return path;
        }
        public string getPlayerPicById(int playerId)
        {
            string path = root + "player-pic/" + playerId + ".png";
            return path;
        }
    }
}
