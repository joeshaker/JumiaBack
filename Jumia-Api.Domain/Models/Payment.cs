using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }  // Equal to Order.FinalAmount

        [MaxLength(100)]
        public string ?PaymobTransactionId { get; set; }  // Optional, for reconciliation

        [MaxLength(100)]
        public string PaymobOrderId { get; set; }        // Optional, from Paymob

        [MaxLength(1000)]
        public string IframeUrl { get; set; }            // Optional, if you're using iframe redirection

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }

        // Navigation
        public virtual Order ?Order { get; set; }
    }
}
