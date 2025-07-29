using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Interfaces.Repositories
{
    public interface ICustomerRepo :IGenericRepo<Customer>
    {
        public Task<Customer?> GetCustomerByUserIdAsync(string userId);
        public Task<bool> ToggleBlockStatusAsync(int customerId);

        public Task<IEnumerable<Customer>> GetAllCustomersAsync();
    }
}
