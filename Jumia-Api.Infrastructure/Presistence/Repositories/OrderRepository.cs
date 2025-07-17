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
    public class OrderRepository:GenericRepo<Order>,IOrderRepository
    {
        public OrderRepository(JumiaDbContext context) : base(context)
        {
        }


        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Where(o => o.CustomerId == customerId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetWithDetailsAsync(int id)
        {
            return await _dbSet
                .Where(o => o.OrderId == id)
                .Include(o => o.Customer)
                .Include(o => o.Address)
                .Include(o => o.Coupon)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(o => o.Customer)
                .Include(o => o.Address)
                .Include(o => o.Coupon)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> CancelOrderAsync(int id, string cancellationReason = null)
        {
            var order = await _dbSet.FindAsync(id);
            if (order == null || order.Status == "cancelled")
                return false;

            order.Status = "cancelled";
            order.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(order);
            return true;
        }
    }
}
