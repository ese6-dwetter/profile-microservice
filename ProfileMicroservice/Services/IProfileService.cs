using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProfileMicroservice.Entities;

namespace ProfileMicroservice.Services
{
    public interface IProfileService
    {
        Task<Profile> CreateProfileAsync(Guid id, string username);
        Task<Profile> GetProfileByIdAsync(Guid id);
        Task<IEnumerable<Profile>> GetProfilesAsync();

        Task<Profile> UpdateProfileAsync(
            string firstName, string lastName, DateTime birthday, string location, string bio, string token
        );

        Task DeleteProfileByIdAsync(Guid id);

        /// <summary>
        /// Add a follower to a profile.
        /// </summary>
        /// <param name="profileId">The GUID of the profile that wants to be followed</param>
        /// <param name="token">JWT of the user that wants to follow a profile</param>
        /// <returns>Profile that is followed</returns>
        Task<Profile> FollowProfileByIdAsync(Guid profileId, string token);

        /// <summary>
        /// Remove a follower from a profile.
        /// </summary>
        /// <param name="profileId">The GUID of the profile that wants to be unfollowed</param>
        /// <param name="token">JWT of the user that wants to follow a profile</param>
        /// <returns>Profile that is unfollowed</returns>
        Task<Profile> UnfollowProfileByIdAsync(Guid profileId, string token);
    }
}
