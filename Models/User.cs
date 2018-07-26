using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace LoginReg.Models
{
    public class User
    {
        [Key]
        public int userid { get; set; }

        [Required(ErrorMessage="First Name is required.")]
        [MinLength(3, ErrorMessage="First Name must be greater than 3 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage="Last Name is required.")]
        [MinLength(3, ErrorMessage="Last Name must be greater than 3 characters.")]
        public string LastName  { get; set; }

        [Required(ErrorMessage="Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required(ErrorMessage="Must confirm password.")]
        [Compare("Password", ErrorMessage="Your passwords must match!")]
        public string confirm_password { get; set; }
        public List<Auction> Auctions {get; set;}
        
        public User()
        {
            Auctions = new List<Auction>();
        }
    }
}