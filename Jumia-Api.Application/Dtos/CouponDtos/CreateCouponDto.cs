using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.CouponDtos
{
    public class CreateCouponDto
    {
        //Used by the Admin or Seller to create a new coupon.


        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal DiscountAmount { get; set; }

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

    }
}
