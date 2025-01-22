using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace LastDance.ViewModels.Account
{
    public class RegisterVM
    {
        [MaxLength(40)]
        [MinLength(3)]
        public string Name { get; set; }
        [MaxLength(40)]
        [MinLength(3)]
        public string Surname { get; set; }
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }
        [MinLength(3)]
        [MaxLength(40)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(256)]
        [MinLength(8)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
         [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
