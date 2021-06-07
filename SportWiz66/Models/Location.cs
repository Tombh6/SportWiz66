using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportWiz66.Models
{
    public class Location
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Location Name")]
        public String Name { get; set; }

        [Required]
        [DisplayName("Full Adress")]
        [StringLength(1024, MinimumLength = 3)]
        public String Adress { get; set; }
    }
}
