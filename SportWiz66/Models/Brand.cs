using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportWiz66.Models
{
    public class Brand
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Brand")]
        public String Name { get; set; }

        [StringLength(2048)]
        public String Description { get; set; }

        public String ImageUrl { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
