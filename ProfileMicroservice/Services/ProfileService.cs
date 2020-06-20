using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProfileMicroservice.Entities;

namespace ProfileMicroservice.Services
{
    public class ProfileService : IProfileService
    {
        public async Task<Profile> GetProfileByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Profile>> GetProfilesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Profile> CreateProfileAsync(Guid id, string username)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProfileByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Profile> AddFollowingToProfileAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Profile> RemoveFollowingFromProfileAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Profile> AddFollowerToProfileAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Profile> RemoveFollowerFromProfileAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
