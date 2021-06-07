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
    public class ShoppingCart
    {
        public int Id { get; set; }
        //acount sessionID used to bind cart to costumer
        public String AccountSessionID { get; set; }

        public ICollection<ShoppingCartItem> Items { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Date created")]
        public DateTime DateCreated { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Last modified")]
        public DateTime LastModified { get; set; }


    }
}
