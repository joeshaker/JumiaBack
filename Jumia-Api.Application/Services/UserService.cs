using AutoMapper;
using Jumia_Api.Application.Dtos.UserDtos;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork , IMapper mapper)
        {
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            var user = await _unitOfWork.UserRepo.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task UpdateUserProfileAsync(string userId, UpdateUserDto updateDto)
        {
            var user = await _unitOfWork.UserRepo.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            _mapper.Map(updateDto, user);
            await _unitOfWork.UserRepo.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
