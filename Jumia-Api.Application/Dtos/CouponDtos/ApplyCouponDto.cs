using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.CouponDtos
{
    public class ApplyCouponDto
    {
        //Used by the Customer when trying to apply a coupon at checkout.

        [Required]
        public string Code { get; set; }

        [Required]
        public decimal CartTotal { get; set; }

    }
}
