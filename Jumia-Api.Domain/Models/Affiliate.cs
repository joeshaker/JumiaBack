using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class Affiliate
    {
        [Key]
        public int AffiliateId { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        [MaxLength(20)]
        public string AffiliateCode { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal CommissionRate { get; set; } = 5.00m;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalEarnings { get; set; } = 0;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal AvailableBalance { get; set; } = 0;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal WithdrawnAmount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public AppUser User { get; set; }
        public ICollection<AffiliateSellerRelationship> SellerRelationships { get; set; }
        public ICollection<AffiliateCommission> Commissions { get; set; }
        public ICollection<AffiliateWithdrawal> Withdrawals { get; set; }
    }
}
