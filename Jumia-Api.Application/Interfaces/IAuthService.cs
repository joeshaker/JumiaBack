using Jumia_Api.Api.Contracts.Results;
using Jumia_Api.Application.Common.Results;
using Jumia_Api.Application.Dtos.AuthDtos;
using Microsoft.AspNetCore.Identity;

namespace Jumia_Api.Application.Interfaces
{
    public interface IAuthService
    {

        Task<AuthResult> LoginAsync(LoginDTO logindto);
    
        Task<AuthResult> RegisterAsync(PasswordSetupDto passsetdto);

        public Task<(bool Success, string Message)> CreateRoleAsync(string roleName);

        public Task<AuthResult> ForgetPasswordAsync(string email);

        public Task<AuthResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

    }
}
