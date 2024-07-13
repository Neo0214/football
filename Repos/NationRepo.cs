using football.Entities;
using Object_Storage;

namespace football.Repos
{
    public class NationRepo
    {
        private readonly FootballContext _context;
        private readonly ObjectStorageContext _OSS;
        public NationRepo(FootballContext context, ObjectStorageContext osc)
        {
            _context = context;
            _OSS = osc;
        }

        public byte[] getPic(int nationId)
        {
            Nation nation = _context.Nations.Single(p => p.NationId == nationId);
            return _OSS.fetchFile(nation.AvatarId);
        }
    }
}
