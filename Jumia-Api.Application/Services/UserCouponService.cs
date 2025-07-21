using AutoMapper;
using Jumia_Api.Application.Dtos.CouponDtos;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Services
{
    public class UserCouponService : IUserCouponService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        public UserCouponService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AssignCouponToUserAsync(int userId, int couponId)
        {
            return await _unitOfWork.UserCouponRepo.AssignCouponToUserAsync(userId, couponId);
        }

        public async Task<bool> DeleteUserCouponAsync(int userCouponId)
        {
            return await _unitOfWork.UserCouponRepo.DeleteUserCouponAsync(userCouponId);

        }

        public async Task<UserCouponDto?> GetUserCouponAsync(int userId, int couponId)
        {
            var coupon = await _unitOfWork.UserCouponRepo.GetUserCouponAsync(userId, couponId);
            if (coupon == null)
            {
                return null; // Coupon not found for the user
            }
            return new UserCouponDto
            {
                UserCouponId = coupon.UserCouponId,
                UserId = coupon.CustomerId,
                CouponId = coupon.CouponId,
                IsUsed = coupon.IsUsed,
                AssignedAt = coupon.AssignedAt,
                UsedAt = coupon.UsedAt,
                CouponCode = coupon.Coupon?.Code,
                DiscountAmount = coupon.Coupon?.DiscountAmount ?? 0,
                DiscountType = coupon.Coupon?.DiscountType
            };

        }

        public async Task<IEnumerable<UserCouponDto>> GetUserCouponsAsync(int userId)
        {
            var coupons = await _unitOfWork.UserCouponRepo.GetUserCouponsAsync(userId);
            //return _mapper.Map<IEnumerable<UserCouponDto>>(coupons);
            return coupons.Select(c => new UserCouponDto
            {
                UserCouponId = c.UserCouponId,
                CouponId = c.CouponId,
                UserId = c.CustomerId,
                IsUsed = c.IsUsed,
                AssignedAt = c.AssignedAt,
                UsedAt = c.UsedAt,
                CouponCode = c.Coupon?.Code,
                DiscountAmount = c.Coupon?.DiscountAmount ?? 0,
                DiscountType = c.Coupon?.DiscountType
            });

        }

        public async Task<bool> IsCouponUsedByUserAsync(int userId, int couponId)
        {
            return await _unitOfWork.UserCouponRepo.IsCouponUsedByUserAsync(userId, couponId);

        }

        public async Task<bool> MarkCouponAsUsedAsync(int userId, int couponId)
        {
            return await _unitOfWork.UserCouponRepo.MarkCouponAsUsedAsync(userId, couponId);

        }
    }
}
