using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class AffiliateCommission
    {
        [Key]
        public int CommissionId { get; set; }

        [Required]
        [ForeignKey("Affiliate")]
        public int AffiliateId { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("SubOrder")]
        public int SubOrderId { get; set; }

        [Required]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        [Required]
        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal CommissionAmount { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal CommissionRate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal OrderItemTotal { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PaidAt { get; set; }

        // Navigation properties
        public Affiliate Affiliate { get; set; }
        public Order Order { get; set; }
        public SubOrder SubOrder { get; set; }
        public Seller Seller { get; set; }
        public OrderItem OrderItem { get; set; }
        public Product Product { get; set; }
    }
}
