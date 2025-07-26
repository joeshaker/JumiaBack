using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.OrderDtos
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public int? CouponId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? AffiliateId { get; set; }
        public string? AffiliateCode { get; set; }
        public string Status { get; set; } = "pending";
        //list of suborders
        public List<SubOrderDTO> SubOrders { get; set; } = new List<SubOrderDTO>();
    }
}
