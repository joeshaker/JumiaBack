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
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }

        public Task AddAdress()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable< Address>> GetAllAdressesByUserId(string userId)
        {
           var addresses = await _unitOfWork.AddressRepo.GetAllAddressByUserIdAsync(userId);

            return addresses;
        }
    }
}
