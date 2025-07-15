using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class ReviewImage
    {
        [Key]
        public int ReviewImageId { get; set; }

        [Required]
        [ForeignKey("Rating")]
        public int RatingId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ImageUrl { get; set; }

        // Navigation property
        public Rating Rating { get; set; }
    }
}
