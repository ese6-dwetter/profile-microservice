using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProfileMicroservice.Entities;

namespace ProfileMicroservice.Services
{
    public interface IProfileService
    {
        Task<Profile> GetProfileByIdAsync(Guid id);
        Task<IEnumerable<Profile>> GetProfilesAsync();
        Task<Profile> CreateProfileAsync(Guid id, string username);
        Task DeleteProfileByIdAsync(Guid id);
        Task<Profile> AddFollowingToProfileAsync(Guid id);
        Task<Profile> RemoveFollowingFromProfileAsync(Guid id);
        Task<Profile> AddFollowerToProfileAsync(Guid id);
        Task<Profile> RemoveFollowerFromProfileAsync(Guid id);
    }
}
