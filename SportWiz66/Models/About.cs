using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SportWiz66.Models
{
    public class About
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        public String Description { get; set; }

        public String ImageUrl { get; set; }
    }
}
