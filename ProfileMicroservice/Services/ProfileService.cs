using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfileMicroservice.Entities;
using ProfileMicroservice.Exceptions;
using ProfileMicroservice.Helpers;
using ProfileMicroservice.Repositories;

namespace ProfileMicroservice.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _repository;
        private readonly ITokenGenerator _tokenGenerator;

        public ProfileService(IProfileRepository repository, ITokenGenerator tokenGenerator)
        {
            _repository = repository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Profile> CreateProfileAsync(Guid id, string username)
        {
            if (id == null || username == null)
                throw new NullReferenceException();

            var profile = await _repository.CreateAsync(new Profile
            {
                Id = id,
                Username = username,
                FirstName = null,
                LastName = null,
                Birthday = new DateTime(),
                Location = null,
                Bio = null,
                Followers = new List<User>(),
                Following = new List<User>()
            });

            return profile;
        }

        public async Task<Profile> GetProfileByIdAsync(Guid id)
        {
            var profile = await _repository.ReadByIdAsync(id)
                          ?? throw new ProfileNotFoundException();

            return profile;
        }

        public async Task<IEnumerable<Profile>> GetProfilesAsync()
        {
            return await _repository.ReadAsync();
        }

        public async Task<Profile> UpdateProfileAsync(string firstName, string lastName, DateTime birthday, string location, string bio, string token)
        {
            var id = Guid.Parse(_tokenGenerator.GetJwtClaim(token, "nameid"));
            if (id == null)
                throw new UnauthorizedAccessException();
            
            var profile = await _repository.ReadByIdAsync(id)
                          ?? throw new ProfileNotFoundException();

            profile.FirstName = firstName;
            profile.LastName = lastName;
            profile.Birthday = birthday;
            profile.Location = location;
            profile.Bio = bio;

            return await _repository.UpdateAsync(id, profile);
        }

        public async Task DeleteProfileByIdAsync(Guid id)
        {
            await _repository.DeleteByIdAsync(id);
        }

        public async Task<Profile> FollowProfileByIdAsync(Guid profileId, string token)
        {
            var id = Guid.Parse(_tokenGenerator.GetJwtClaim(token, "nameid"));
            var username = _tokenGenerator.GetJwtClaim(token, "unique_name");

            if (id == null || username == null)
                throw new UnauthorizedAccessException();

            var profile = await _repository.ReadByIdAsync(id)
                          ?? throw new ProfileNotFoundException();

            var profileToFollow = await _repository.ReadByIdAsync(profileId)
                                  ?? throw new ProfileNotFoundException();

            if (profile.Following.ToList().Find(x => x.Id == profileToFollow.Id) != null
                || profileToFollow.Followers.ToList().Find(x => x.Id == profile.Id) != null)
                throw new AlreadyFollowingException();
            
            profile.Following.ToList().Add(new User
            {
                Id = profileToFollow.Id,
                Username = profileToFollow.Username
            });
            
            profileToFollow.Followers.ToList().Add(new User
            {
                Id = profile.Id,
                Username = profile.Username
            });

            await _repository.UpdateAsync(id, profile);
            return await _repository.UpdateAsync(profileId, profileToFollow);
        }

        public async Task<Profile> UnfollowProfileByIdAsync(Guid profileId, string token)
        {
            var id = Guid.Parse(_tokenGenerator.GetJwtClaim(token, "nameid"));
            var username = _tokenGenerator.GetJwtClaim(token, "unique_name");

            if (id == null || username == null)
                throw new UnauthorizedAccessException();

            var profile = await _repository.ReadByIdAsync(id)
                          ?? throw new ProfileNotFoundException();

            var profileToFollow = await _repository.ReadByIdAsync(profileId)
                                  ?? throw new ProfileNotFoundException();
            
            profile.Following.ToList().RemoveAll(x => x.Id == profileToFollow.Id);

            profileToFollow.Followers.ToList().RemoveAll(x => x.Id == profile.Id);

            await _repository.UpdateAsync(id, profile);
            return await _repository.UpdateAsync(profileId, profileToFollow);
        }
    }
}
