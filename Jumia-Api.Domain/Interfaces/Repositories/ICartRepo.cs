using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Interfaces.Repositories
{
    public interface ICartRepo:IGenericRepo<Cart>
    {
        public Task<Cart?> GetCustomerCartAsync(int customerId);
        public Task ClearCartAsync(int customerId);

    }
}
