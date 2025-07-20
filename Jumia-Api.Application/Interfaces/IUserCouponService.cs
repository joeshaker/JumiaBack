using Jumia_Api.Application.Dtos.CouponDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Interfaces
{
    public interface IUserCouponService
    {
        Task<bool> AssignCouponToUserAsync(int userId, int couponId);
        Task<bool> MarkCouponAsUsedAsync(int userId, int couponId);
        Task<IEnumerable<UserCouponDto>> GetUserCouponsAsync(int userId);
        Task<UserCouponDto?> GetUserCouponAsync(int userId, int couponId);
        Task<bool> IsCouponUsedByUserAsync(int userId, int couponId);
        Task<bool> DeleteUserCouponAsync(int userCouponId);

    }
}
