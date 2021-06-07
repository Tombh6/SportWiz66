using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportWiz66.Models
{
    public class Category
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Category")]
        [StringLength(160, MinimumLength = 2)]
        public String Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
