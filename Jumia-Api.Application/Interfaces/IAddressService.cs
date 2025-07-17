using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Interfaces
{
    public interface IAddressService
    {
        public  Task<IEnumerable< Address>> GetAllAdressesByUserId(string userId);
        public Task AddAdress();
    }
}
