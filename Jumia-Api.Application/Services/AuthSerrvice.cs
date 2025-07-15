using Jumia_Api.Api.Contracts.Results;
using Jumia_Api.Application.Common.Results;
using Jumia_Api.Application.Dtos.AuthDtos;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
        IConfiguration configuration,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.configuration = configuration;
            this._roleManager = roleManager;
        }


        public async Task<Result<AuthResult>> Asynclogin(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            
            var roles = await _userManager.GetRolesAsync(user);
            var authResult = GenerateJwtToken(user, roles);
            return Result<AuthResult>.Success(authResult);
        }

        //public Task<AuthResult> Asynclogout(logoutDTO logoutDTO)
        //{
        //    return Task.FromResult(new AuthResult
        //    {
        //        Successed = true,
        //        Message = "Logout Successful"
        //    });
        //}

        public async Task<Result<AuthResult>> Asyncregister(RegisterDTO registerDTO)
        {
            var userExists = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (userExists != null)
            {
                return Result<AuthResult>.Failure("this email already exists");
            }
            var user = new AppUser
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email
            };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                return Result<AuthResult>.Failure($"Registration failed: {errors}");
            }
            await _userManager.AddToRoleAsync(user, "Customer");
            var authResult = new AuthResult
            {
                Successed = true,
                Message = "Registration successful",
            };
            return  Result<AuthResult>.Success(authResult);

        }

        public async Task<Result<string>> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return Result<string>.Failure("Role name cannot be empty.");

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return Result<string>.Failure($"Role '{roleName}' already exists.");

            var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            if (!result.Succeeded)
            {

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<string>.Failure($"Failed to create role: {errors}");
            }

                return Result<string>.Failure($"Role '{roleName}' created successfully.");
        }

        private AuthResult GenerateJwtToken(AppUser user, IList<string> roles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]));

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Jwt:DurationInMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256));

            return new AuthResult
            {
                Successed = true,
                Message = "Token generated successfully",
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
