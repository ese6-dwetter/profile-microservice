using System;
using System.Collections.Generic;
namespace ProfileMicroservice.Entities
{
    public class Profile : BaseEntity
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
        public IEnumerable<User> Followers { get; set; }
        public IEnumerable<User> Following { get; set; }
    }
}
