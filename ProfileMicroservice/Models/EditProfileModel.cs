using System;
using System.ComponentModel.DataAnnotations;

namespace ProfileMicroservice.Models
{
    public class EditProfileModel
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public DateTime Birthday { get; set; }
        [Required] public string Location { get; set; }
        [Required] public string Bio { get; set; }
    }
}
