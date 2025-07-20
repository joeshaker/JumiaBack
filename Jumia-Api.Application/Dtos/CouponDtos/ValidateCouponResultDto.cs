using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.CouponDtos
{
    public class ValidateCouponResultDto
    {
        //Returned as the result of applying/validating a coupon (e.g., success, failure reason, discount value)


        public bool IsValid { get; set; }
        public string Message { get; set; }
        public decimal? DiscountValue { get; set; }
        public string DiscountType { get; set; }


    }
}
