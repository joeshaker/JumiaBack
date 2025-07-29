using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public DateTime? LastLogin { get; set; }

        public bool IsBlocked { get; set; }


        // Navigation property
        public AppUser User { get; set; }
    }
}
