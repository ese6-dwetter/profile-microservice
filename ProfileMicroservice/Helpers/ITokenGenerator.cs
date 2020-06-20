using System;

namespace ProfileMicroservice.Helpers
{
    public interface ITokenGenerator
    {
        /// <summary>
        ///     Generates a new JSON Web Token with the user identity.
        /// </summary>
        /// <param name="userId">Guid of the user</param>
        /// <param name="email">Email of the user</param>
        /// <param name="username">Username of the user</param>
        /// <returns>JSON Web Token</returns>
        string GenerateJwt(Guid userId, string email, string username);

        /// <summary>
        ///     Gets claims from a JSON Web Token
        /// </summary>
        /// <param name="token">Token of the Bearer</param>
        /// <param name="claimType">Claim to validate with</param>
        /// <returns>JSON Web Token</returns>
        string GetJwtClaim(string token, string claimType);
    }
}
