using System;

namespace ProfileMicroservice.Exceptions
{
    [Serializable]
    public class ProfileNotFoundException : Exception
    {
        public ProfileNotFoundException()
            : base("This profile was not found.")
        {
        }
    }
}
