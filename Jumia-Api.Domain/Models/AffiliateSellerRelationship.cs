using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class AffiliateSellerRelationship
    {
        [Key]
        public int RelationshipId { get; set; }

        [Required]
        [ForeignKey("Affiliate")]
        public int AffiliateId { get; set; }

        [Required]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? CommissionRate { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Affiliate Affiliate { get; set; }
        public Seller Seller { get; set; }
    }
}
