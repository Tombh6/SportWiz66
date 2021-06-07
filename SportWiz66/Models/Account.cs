using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SportWiz66.Models
{
    public enum UserType
    {
        Admin,
        Customer
    }

    public class Account
    {

        /// <--------------------->
        /// Account Details
        /// <--------------------->
        [DisplayName("ID")]
        public int Id { get; set; }

        [Required]
        [DisplayName("User name")]
        public string Username { get; set; }

        [Required]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The passwords you entered do not match.")]
        public string ConfirmPassword { get; set; }


        public UserType Type { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Registration date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime RegistrationDate { get; set; }



        /// <--------------------------------------------------------->
        /// misc: Account Orders, Reviews List, Location & Payment Info By Da-vid.M
        /// <--------------------------------------------------------->

        [DisplayName("Your orders")]
        public ICollection<Order> Orders { get; set; }

        [DisplayName("Your Revies")]
        public ICollection<Review> Reviews { get; set; }


        public ICollection<ProductViews> ProductViews { get; set; }
    }
}
