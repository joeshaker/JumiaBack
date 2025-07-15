using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class HelpfulRating
    {
        [Key]
        public int HelpfulId { get; set; }

        [Required]
        [ForeignKey("Rating")]
        public int RatingId { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required]
        public bool IsHelpful { get; set; }

        // Navigation properties
        public Rating Rating { get; set; }
        public Customer Customer { get; set; }
    }
}
