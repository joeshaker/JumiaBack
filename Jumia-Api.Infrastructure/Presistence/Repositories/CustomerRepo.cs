using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    public class CustomerRepo : GenericRepo<Customer>, ICustomerRepo
    {
        public CustomerRepo(JumiaDbContext context) : base(context)
        {
        }

        public async Task<Customer?> GetCustomerByUserIdAsync(string userId)
        {
            return await _dbSet.Where(c=>c.UserId==userId).FirstOrDefaultAsync();
        }
    }
}
