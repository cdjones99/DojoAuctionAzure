using System;
using System.ComponentModel.DataAnnotations;
namespace LoginReg.Models
{
    public class Auction
    {
        [Key]
        public int auctionid { get; set; }

        [Required(ErrorMessage="Idea is required. It must be between 5 and 255 characters.")]
        [MaxLength(255)]
        [MinLength(5)]
        public string ProductName { get; set; }

        [Required(ErrorMessage="Description is required.")]
        [MaxLength(400, ErrorMessage="Length must not exceed 400 characters.")]
        [MinLength(5, ErrorMessage="Length must be atleast 5 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage="Date is required.")]
        public DateTime EndDate {get; set;}

        [Required(ErrorMessage="Bid is required.")]
        public int Bid { get; set; }

        public int userid { get; set; }

        public User user { get; set; }
    }
}