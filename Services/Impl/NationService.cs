using football.Repos;
using football.Services;

namespace football.Services.Impl
{
    public class NationService : INation
    {
        private readonly NationRepo _repo;
        public NationService(NationRepo repo)
        {
            _repo = repo;
        }

        public byte[] getPic(int nationId)
        {
            return _repo.getPic(nationId);
        }
    }
}
