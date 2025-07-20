using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.CouponDtos
{
    public class UserCouponDto
    {
        public int UserCouponId { get; set; }
        public int UserId { get; set; }
        public int CouponId { get; set; }
        public bool IsUsed { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? UsedAt { get; set; } // Nullable if not used yet



        public string CouponCode { get; set; } // Code of the coupon
        public decimal DiscountAmount { get; set; } // Amount of discount provided by the coupon
        public string DiscountType { get; set; } // Type of discount (e.g., percentage, fixed amount)
        public DateTime StartDate { get; set; } // Start date of the coupon validity
        public DateTime EndDate { get; set; } // End date of the coupon validity

        public string CouponDescription { get; set; } // Description of the coupon


    }
}
