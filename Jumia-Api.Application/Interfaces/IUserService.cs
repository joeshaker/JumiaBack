using Jumia_Api.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string email);

        Task<IdentityResult> CreateUserAsync(string email, string password);
        Task<AppUser> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
    }
}
