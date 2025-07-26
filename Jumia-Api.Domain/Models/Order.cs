using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required]
        [ForeignKey("Address")]
        public int AddressId { get; set; }

        [ForeignKey("Coupon")]
        public int? CouponId { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [Required]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal ShippingFee { get; set; } = 0;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TaxAmount { get; set; } = 0;

        [Column(TypeName = "decimal(10, 2)")]
        [Required]
        public decimal FinalAmount { get; set; }

        [Required]
        [MaxLength(20)]
        public string PaymentMethod { get; set; }

        [MaxLength(10)]
        public string PaymentStatus { get; set; } = "pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        public string Status { get; set; } = "pending";

        [ForeignKey("Affiliate")]
        public int? AffiliateId { get; set; }

       
        public string? AffiliateCode { get; set; } ="N/A";

        // Navigation properties
        public Customer Customer { get; set; }
        public Address Address { get; set; }
        public Coupon Coupon { get; set; }
        public Affiliate Affiliate { get; set; }
        public ICollection<SubOrder> SubOrders { get; set; }
        //public Payment Payment { get; set; }
    }
}
