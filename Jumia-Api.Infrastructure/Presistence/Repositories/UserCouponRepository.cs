using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    public class UserCouponRepository : GenericRepo<UserCoupon> ,IUserCouponRepo
    {
        public UserCouponRepository(JumiaDbContext context) : base(context)
        {
        }

        public async Task<bool> AssignCouponToUserAsync(int userId, int couponId)
        {
           var exists = await _dbSet.AnyAsync(uc => uc.CustomerId == userId && uc.CouponId == couponId);
            if (exists) 
            {
                return false; // Coupon already assigned to user
            }
            var userCoupon = new UserCoupon
            {
                CustomerId = userId,
                CouponId = couponId,
                IsUsed = false,
                AssignedAt = DateTime.UtcNow
            };
            await _dbSet.AddAsync(userCoupon);
            return await _context.SaveChangesAsync() > 0; // Returns true if save was successful
        }

        public async Task<bool> DeleteUserCouponAsync(int userCouponId)
        {
            var userCoupon = await _dbSet.FindAsync(userCouponId);
            if (userCoupon == null)
            {
                return false; // User coupon not found
            }
            _dbSet.Remove(userCoupon);
            return await _context.SaveChangesAsync() > 0; // Returns true if save was successful

        }

        public async Task<UserCoupon?> GetUserCouponAsync(int userId, int couponId)
        {
            
            return  await _dbSet.Include(uc => uc.Coupon)
                .FirstOrDefaultAsync(uc => uc.CustomerId == userId && uc.CouponId == couponId);

        }

        public async Task<IEnumerable<UserCoupon>> GetUserCouponsAsync(int userId)
        {
            // for "My Coupons" page -> Fetch all user coupons for the given userId, including related Coupon details
            return await _dbSet.Include(uc => uc.Coupon).Where(uc => uc.CustomerId == userId).ToListAsync();

        }

        public async Task<bool> IsCouponUsedByUserAsync(int userId, int couponId)
        {
            var userCoupon = await _dbSet.FirstOrDefaultAsync(uc => uc.CustomerId == userId && uc.CouponId == couponId);
            return userCoupon != null && userCoupon.IsUsed; // Returns true if the coupon is used by the user

        }

        public async Task<bool> MarkCouponAsUsedAsync(int userId, int couponId)
        {
            var userCoupon = await _dbSet.FirstOrDefaultAsync(uc => uc.CustomerId == userId && uc.CouponId == couponId);
            if (userCoupon == null || userCoupon.IsUsed)
            {
                return false; // Coupon not found or already used
            }
            userCoupon.IsUsed = true;
            userCoupon.UsedAt = DateTime.UtcNow;
            _dbSet.Update(userCoupon);
            return await _context.SaveChangesAsync() > 0; // Returns true if save was successful 

        }
    }
}
