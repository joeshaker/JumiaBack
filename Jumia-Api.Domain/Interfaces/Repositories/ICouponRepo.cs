using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Interfaces.Repositories
{
    public interface ICouponRepo : IGenericRepo<Coupon>
    {
        Task<Coupon?> GetByCodeAsync (string code);
        Task<IEnumerable<Coupon>> GetAllActiveAsync();
        Task CreateAsync(Coupon coupon);
        Task<bool> IncrementUsageAsync(string code);
        
    }
}
