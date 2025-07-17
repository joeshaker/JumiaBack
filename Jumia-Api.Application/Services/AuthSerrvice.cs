using Jumia_Api.Application.Dtos.AuthDtos;
using Jumia_Api.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Jumia_Api.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IOtpService _otpService;

        public AuthService(IUserService userService, IJwtService jwtService, IOtpService otpService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _otpService = otpService;
        }

        public async Task<(bool Success, string Token, string Message)> LoginAsync(LoginDTO dto)
        {
            var user = await _userService.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return (false, null, "User not found");
            }
            var passwordValid = await _userService.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
            {
                return (false, null, "Invalid password");
            }
            var token = await _jwtService.GenerateJwtTokenAsync(user);
            return (true, token, "Login successful");

        }

        public async Task<(bool Success,string Token, string Message)> RegisterAsync(PasswordSetupDto dto)
        {
            if(dto.Password != dto.ConfirmPassword)
            {
                return (false, null,"Passwords do not match");
            }

            var otpValid = _otpService.ValidateOtp(dto.Email, dto.OtpCode);
            if (!otpValid)
            {
                return (false,null, "Invalid or expired OTP code");
            }

            var result = await _userService.CreateUserAsync(dto.Email, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                return (false,null, $"User creation failed: {errors}");
            }

            _otpService.RemoveOtp(dto.Email);

            //Generate token after registeration to allow user to login immediately
            var user = await _userService.FindByEmailAsync(dto.Email);
            var token = await _jwtService.GenerateJwtTokenAsync(user);

            return (true, token,"User registered successfully");

        }

     

    }
}
