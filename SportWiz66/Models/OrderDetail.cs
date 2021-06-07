using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SportWiz66.Models
{
    //This class represents "Order Item"(The Product) inside Order
    public class OrderDetail
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        public Order AssociatedOrder { get; set; }
        public Product Product { get; set; }

        [Required]
        [DisplayName("Price")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")] //rule: Price: 18 digits, 2 decimal points , without that we get an error 
        public decimal UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

    }
}
