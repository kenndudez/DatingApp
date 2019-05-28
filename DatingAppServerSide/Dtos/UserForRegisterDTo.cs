using System.ComponentModel.DataAnnotations;

namespace DatingAppServerSide.Dtos
{
    public class UserForRegisterDTo
    {
         [Required]
        public string Username { get; set;}
        [Required]
        [StringLength(8, MinimumLength= 4, ErrorMessage= "You Register with a invalid password")]
        public string Password {get; set;}
    }
}