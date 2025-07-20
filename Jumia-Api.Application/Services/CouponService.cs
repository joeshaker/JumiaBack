using AutoMapper;
using Jumia_Api.Application.Dtos.CouponDtos;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Services
{
    public class CouponService : ICouponService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CouponService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> ApplyCouponAsync(string code, decimal cartTotal)
        {
            var coupon = await _unitOfWork.CouponRepo.GetByCodeAsync(code);
            if (coupon == null || !coupon.IsActive || coupon.StartDate > DateTime.UtcNow || coupon.EndDate < DateTime.UtcNow)
            {
                return false; // Coupon is not valid
            }
            if (coupon.MinimumPurchase > cartTotal)
            {
                return false; // Order total does not meet minimum purchase requirement
            }
            // Check usage limit
            if (coupon.UsageLimit.HasValue && coupon.UsageCount >= coupon.UsageLimit.Value)
            {
                return false; // Coupon usage limit reached
            }
            // Increment usage count
            await _unitOfWork.CouponRepo.IncrementUsageAsync(code);
            return true; // Coupon applied successfully

        }

        public async Task<bool> CreateCouponAsync(CreateCouponDto dto)
        {
            var coupon = _mapper.Map<Coupon>(dto);
            await _unitOfWork.CouponRepo.CreateAsync(coupon);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCouponAsync(int couponId)
        {
            var coupon = await _unitOfWork.CouponRepo.GetByIdAsync(couponId);
            if (coupon == null)
            {
                return false; // Coupon not found
            }
            coupon.IsActive = false; //soft delete
            _unitOfWork.CouponRepo.Update(coupon);
            return await _unitOfWork.SaveChangesAsync() > 0;
            
        }

        public async Task<CouponDto?> GetCouponByCodeAsync(string code)
        {
            var coupon = await _unitOfWork.CouponRepo.GetByCodeAsync(code);
            if(coupon == null || !coupon.IsActive || coupon.StartDate > DateTime.UtcNow || coupon.EndDate < DateTime.UtcNow)
            {
                return null;
            }
            return _mapper.Map<CouponDto>(coupon);

        }

        public async Task<IEnumerable<CouponDto>> GettAllActiveCouponAsync()
        {
            var coupons = await _unitOfWork.CouponRepo.GetAllActiveAsync();
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }

        public async Task<bool> IncrementUsageAsync(string code)
        {
            return await _unitOfWork.CouponRepo.IncrementUsageAsync(code);
        }

        public async Task<bool> UpdateCouponAsync(int couponId, CreateCouponDto dto)
        {
            var coupon = await _unitOfWork.CouponRepo.GetByIdAsync(couponId);
            if (coupon == null)
            {
                return false; // Coupon not found
            }
            // Map the updated properties from dto to coupon
            _mapper.Map(dto, coupon);
            _unitOfWork.CouponRepo.Update(coupon);
            return await _unitOfWork.SaveChangesAsync() > 0;

        }
    }
}
