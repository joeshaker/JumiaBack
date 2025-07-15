using Jumia_Api.Api.Contracts.Results;
using Jumia_Api.Application.Common.Results;
using Jumia_Api.Application.Dtos.AuthDtos;

namespace Jumia_Api.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResult>> Asynclogin(LoginDTO loginDTO);
       
        Task<Result<AuthResult>> Asyncregister(RegisterDTO registerDTO);
        Task<Result<string>> CreateRoleAsync(string roleName);
    }
}
