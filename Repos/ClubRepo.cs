using football.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Object_Storage;

namespace football.Repos
{
    public class ClubRepo
    {
        private readonly FootballContext _context;
        private readonly ObjectStorageContext _OSS;
        public ClubRepo(FootballContext context, ObjectStorageContext objectStorageContext)
        {
            _context = context;
            _OSS = objectStorageContext;
        }
        public int getClubIdByUserId(int id)
        {
            string query = "select club_id from user_club where user_id=@userId;";
            var connection = _context.Database.GetDbConnection();
            int clubId = -1;
            try
            {
                connection.Open();
                using (var command = new MySqlCommand(query, (MySqlConnection)connection))
                {
                    command.Parameters.Add(new MySqlParameter("@userId", id));
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            clubId = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
            return clubId;
        }
        public string getClubNameById(int clubId)
        {
            Club club = _context.Clubs.Single(club => club.ClubId == clubId);
            return club.ClubName;
        }
        public byte[] getClubLogo(int clubId)
        {
            // get club avatar id
            string avatarId = _context.Clubs.Where(club => club.ClubId == clubId).Select(club => club.AvatarId).FirstOrDefault();
            // get club avatar  
            return _OSS.fetchFile(avatarId);
        }
        public List<Club> getAllClub()
        {
            return _context.Clubs.ToList();
        }
    }
}
