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
    public class AddressRepo : GenericRepo<Address>, IAddressRepo
    {
        public AddressRepo(JumiaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable< Address?>> GetAllAddressByUserIdAsync(string userId)
        {
            var adresses = await _dbSet.Where(a => a.UserId == userId).ToListAsync();

            return adresses;
        }
    }
}
