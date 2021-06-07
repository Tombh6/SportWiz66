using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportWiz66.Models
{
    public class Review
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        public Product Product { get; set; }

        [DisplayName("sent by")]
        public Account SentBy { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public String Title { get; set; }

        [Required]
        [StringLength(2048)]
        public String Content { get; set; }


    }
}
