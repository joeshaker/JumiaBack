using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Interfaces.Repositories
{
    public interface IUserCouponRepo
    {
        Task<bool> AssignCouponToUserAsync(int userId, int couponId);
        Task<bool> MarkCouponAsUsedAsync(int userId, int couponId);
        Task<IEnumerable<UserCoupon>> GetUserCouponsAsync(int userId);
        Task<UserCoupon?> GetUserCouponAsync(int userId, int couponId);
        Task<bool> IsCouponUsedByUserAsync(int userId, int couponId);
        Task<bool> DeleteUserCouponAsync(int userCouponId);
    }
}
