using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class AffiliateWithdrawal
    {
        [Key]
        public int WithdrawalId { get; set; }

        [Required]
        [ForeignKey("Affiliate")]
        public int AffiliateId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        public string PaymentDetails { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "pending";

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ProcessedAt { get; set; }

        public string Notes { get; set; }

        // Navigation property
        public Affiliate Affiliate { get; set; }
    }
}
