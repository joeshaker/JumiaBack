using Jumia_Api.Application.Dtos.AddressDtos;
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



        // Retrieves all addresses of all users
        public async Task<IEnumerable<AddressDto>> GetAllAddressesOfAllUsers()
        {
            var addresses = await _unitOfWork.AddressRepo.GetAllAddressesOfAllUsersAsync();
            // Map Address entities to AddressDto objects
            var addressDtos = addresses.Select(address => new AddressDto
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                StreetAddress = address.StreetAddress,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                PhoneNumber = address.PhoneNumber,
                IsDefault = address.IsDefault,
                AddressName = address.AddressName
            });
            return addressDtos;
        }
        
        
            
        // Retrieves all addresses for a specific user by their user ID
        public async Task<IEnumerable<AddressDto>> GetAllAddressesByUserId(string userId)
        {
            var addresses = await _unitOfWork.AddressRepo.GetAllAddressesByUserIdAsync(userId);

            // Map Address entities to AddressDto objects
            var addressDtos = addresses.Select(address => new AddressDto
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                StreetAddress = address.StreetAddress,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                PhoneNumber = address.PhoneNumber,
                IsDefault = address.IsDefault,
                AddressName = address.AddressName
            });

            return addressDtos;
        }



        // Retrieves a specific address by its ID
        public async Task<AddressDto> GetAddressById(int addressId)
        {
            var address = await _unitOfWork.AddressRepo.GetAddressByIdAsync(addressId);
            if (address == null )
            {
                throw new KeyNotFoundException($"Address with ID {addressId} not found.");
            }
            // Map Address entity to AddressDto object
            var addressDto = new AddressDto
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                StreetAddress = address.StreetAddress,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                PhoneNumber = address.PhoneNumber,
                IsDefault = address.IsDefault,
                AddressName = address.AddressName
            };
            return addressDto;
        }



        // Adds a new address
        public async Task<AddressDto> AddNewAddress(CreateAddressDto address)
        {
            // Map CreateAddressDto to Address entity
            var newAddress = new Address
            {
                UserId = address.UserId,
                StreetAddress = address.StreetAddress,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                PhoneNumber = address.PhoneNumber,
                IsDefault = address.IsDefault,
                AddressName = address.AddressName
            };

            var addedAddress = await _unitOfWork.AddressRepo.AddNewAddressAsync(newAddress);
            if (addedAddress == null)
            {
                throw new Exception("Failed to add address.");
            }

            // Map Address entity to AddressDto object
            var addressDto = new AddressDto
            {
                AddressId = addedAddress.AddressId,
                UserId = addedAddress.UserId,
                StreetAddress = addedAddress.StreetAddress,
                City = addedAddress.City,
                State = addedAddress.State,
                PostalCode = addedAddress.PostalCode,
                Country = addedAddress.Country,
                PhoneNumber = addedAddress.PhoneNumber,
                IsDefault = addedAddress.IsDefault,
                AddressName = addedAddress.AddressName
            };

            return addressDto;
        }




        // Updates an existing address by its ID
        public async Task<bool> UpdateAddress(int addressId, UpdateAddressDto addressDto)
        {
            var existingAddress = await _unitOfWork.AddressRepo.GetByIdAsync(addressId);
            if (existingAddress == null)
            {
                throw new KeyNotFoundException($"Address with ID {addressId} not found.");
            }
            // Map UpdateAddressDto to Address entity
            existingAddress.StreetAddress = addressDto.StreetAddress;
            existingAddress.City = addressDto.City;
            existingAddress.State = addressDto.State;
            existingAddress.PostalCode = addressDto.PostalCode;
            existingAddress.Country = addressDto.Country;
            existingAddress.PhoneNumber = addressDto.PhoneNumber;
            existingAddress.IsDefault = addressDto.IsDefault;
            existingAddress.AddressName = addressDto.AddressName;

            var success = await _unitOfWork.AddressRepo.UpdateAddressAsync(existingAddress);
            return success;
        }




        // Deletes an address by its ID
        public async Task<bool> DeleteAddress(int addressId)
        {
            var existingAddress = await _unitOfWork.AddressRepo.GetByIdAsync(addressId);
            if (existingAddress == null)
                return false;

            var success = await _unitOfWork.AddressRepo.DeleteAddressAsync(existingAddress);
            return success;
        }




    }
}
