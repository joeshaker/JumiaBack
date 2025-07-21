using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal MinimumPurchase { get; set; } = 0;

        [Required]
        [MaxLength(10)]
        public string DiscountType { get; set; } // 'percentage' or 'fixed'

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int? UsageLimit { get; set; }
        public int UsageCount { get; set; } = 0;

        // Navigation property
        public ICollection<UserCoupon> UserCoupons { get; set; }
    }
}
