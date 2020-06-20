using ProfileMicroservice.Entities;
using ProfileMicroservice.Settings;

namespace ProfileMicroservice.Repositories
{
    public class ProfileRepository : BaseRepository<Profile>, IProfileRepository
    {
        public ProfileRepository(IDatabaseSettings databaseSettings)
            : base(databaseSettings)
        {
        }
    }
}
