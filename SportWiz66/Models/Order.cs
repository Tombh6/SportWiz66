using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SportWiz66.Models
{

    public enum OrderStatus
    {
        Canceled,
        Processing,
        Shipped,
        Delivered,
        Returned
    }


    public class Order
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime OrderDate { get; set; }

        [DisplayName("Total")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")] //rule: Price: 18 digits, 2 decimal points , without that we get an error 
        public decimal TotalPrice { get; set; }

        [Range(0, int.MaxValue)]
        [DisplayName("Items")]
        public int NumOfItems { get; set; }

        [DisplayName("Order status")]
        public OrderStatus Status { get; set; }

        public ICollection<OrderDetail> OrderDetalis { get; set; }



        /// <------------------------------>
        /// Account, Shipping & Payment Info
        /// <------------------------------>

        public Account AssociatedAccount { get; set; }

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
        [DisplayName("Street address")]
        public string StreetAddress { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Credit card number")]
        public string CreditCardNum { get; set; }
    }
}
