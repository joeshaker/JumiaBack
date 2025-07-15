using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class UserCoupon
    {
        [Key]
        public int UserCouponId { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required]
        [ForeignKey("Coupon")]
        public int CouponId { get; set; }

        public bool IsUsed { get; set; } = false;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UsedAt { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }
        public Coupon Coupon { get; set; }
    }
}
