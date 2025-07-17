
﻿using Jumia_Api.Application.Dtos.AuthDtos;


﻿using AutoMapper;
using Jumia_Api.Application.Dtos.UserDtos;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.UnitOfWork;



using Jumia_Api.Domain.Models;
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Services
{

    public class UserService: IUserService
    {
   
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UserService(IUnitOfWork unitOfWork , IMapper mapper,UserManager<AppUser> userManager)
        {

             _userManager = userManager;
              _unitOfWork = unitOfWork;
           _mapper = mapper;

        }
        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        
         public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task<IdentityResult> CreateUserAsync(string email, string password)
        {
            var user = new AppUser
            {
                UserName = email,
                Email = email,
                // Default values for testing
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(2000, 1, 1),
                Address = "123 Main Street",
                Gender = "Female" // or "Male", "Other", etc.
            };
            return await _userManager.CreateAsync(user, password);

        }

        public Task<AppUser> FindByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;

        }
          public async Task UpdateUserProfileAsync(string userId, UpdateUserDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            _mapper.Map(updateDto, user);
            user.Id = userId; 
            await _userManager.UpdateAsync(user);
           
        }
        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> UpdateUserAsync(AppUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

    }
    }
