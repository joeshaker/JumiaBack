using Jumia_Api.Application.Dtos.CouponDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Interfaces
{
    public interface ICouponService
    {
        Task<IEnumerable<CouponDto>> GettAllActiveCouponAsync();

        Task<CouponDto?> GetCouponByCodeAsync(string code);

        Task<bool> ApplyCouponAsync(string code, decimal orderTotal);

        Task<bool> CreateCouponAsync(CreateCouponDto dto);

        Task<bool> IncrementUsageAsync(string code);
        Task<bool> DeleteCouponAsync(int couponId);
        Task<bool> UpdateCouponAsync(int couponId, CreateCouponDto dto);

    }
}
