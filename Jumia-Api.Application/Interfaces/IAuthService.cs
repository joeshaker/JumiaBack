using Jumia_Api.Api.Contracts.Results;
using Jumia_Api.Application.Common.Results;
using Jumia_Api.Application.Dtos.AuthDtos;
using Microsoft.AspNetCore.Identity;

namespace Jumia_Api.Application.Interfaces
{
    public interface IAuthService
    {

        Task<(bool Success, string Token, string Message)> LoginAsync(LoginDTO logindto);
    
        Task<(bool Success, string Token, string Message)> RegisterAsync(PasswordSetupDto passsetdto);

  



    }
}
