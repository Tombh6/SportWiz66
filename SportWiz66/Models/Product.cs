using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SportWiz66.Models
{
    public class Product
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Product")]
        public String Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")] //rule: Price: 18 digits, 2 decimal points , without that we get an error 
        public decimal Price { get; set; }

        [StringLength(2048)]
        [DisplayName("Description")]
        public String Description { get; set; }

        public String ImageUrl { get; set; }

        public String FrontImageUrl { get; set; }
        public String BackImageUrl { get; set; }

        [DisplayName("In stock")]
        public bool InStock { get; set; }

        [Range(0, int.MaxValue)]
        [DisplayName("Purchases")]
        public int TotalPurchases { get; set; }


        public Brand Brand { get; set; }

        public Category Category { get; set; }
        public ICollection<Review> Reviews { get; set; }

        public ICollection<ProductViews> ProductViews { get; set; }

    }
}
