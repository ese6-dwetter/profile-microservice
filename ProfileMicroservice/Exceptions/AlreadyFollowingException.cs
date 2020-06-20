using System;

namespace ProfileMicroservice.Exceptions
{
    [Serializable]
    public class AlreadyFollowingException : Exception
    {
        public AlreadyFollowingException()
            : base("You are already following this profile.")
        {
        }
    }
}
