using Jumia_Api.Application.Dtos.AuthDtos;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jumia_Api.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly UserManager<AppUser> _userManager;
        public AuthController(IOtpService otpService, IEmailService emailService, IUserService userService, IAuthService authService, IJwtService jwtService, UserManager<AppUser> userManager)
        {
            _otpService = otpService;
            _emailService = emailService;
            _userService = userService;
            _authService = authService;
            _jwtService = jwtService;
            _userManager = userManager;
        }

        [HttpPost("email-check")]
        public async Task<IActionResult> CheckEmail([FromBody]EmailCheckDto dto)
        {
            if(await _userService.UserExistsAsync(dto.Email))
            {
                return Ok(new {isRegistered = true, message = "Email already registered"});
            }
            var otp = _otpService.GenerateOtp(dto.Email);
            await _emailService.SendEmailAsync(dto.Email, "Your OTP Code", $"Your OTP code is: {otp}");

            return Ok(new {isRegistered = false, message = "Email not registered, OTP sent", otp });
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] OtpVerifyDto dto)
        {
            var isValid = _otpService.ValidateOtp(dto.Email, dto.OtpCode);

            if (!isValid)
            {
                return BadRequest(new { message = "Invalid or expired OTP code" });
            }
            return Ok(new { otpValid = true });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] PasswordSetupDto dto)
        {
            var (success,token, message) = await _authService.RegisterAsync(dto);

            if (!success)
            {
                return BadRequest(new { message });
            }
            SetJwtCookie(token);
            // in the front-end logic, after the user registers, direct them to the update personal details page if isFirstTimeLogin is true.
            return Ok(new { message , isFirstTimeLogin = true});
           

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var (success, token, message) = await _authService.LoginAsync(dto);

            if (!success)
            {
                return Unauthorized(new { message });
            }


            return Ok(new { message, token });

        }

        [Authorize]
        [HttpPost("update-personal-details")]
        public async Task<IActionResult> UpdatePersonalDetails([FromBody] PersonalDetailsDto dto)
        {
            var userId = GetCurrentUserId();

            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid User ID" });
            }
            var result  = await _authService.UpdatePersonalDetailsAsync(userId, dto);
            
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = $"Failed to update personal details: {errors}" });
            }

            var user = await _userService.GetUserByIdAsync(userId);
            var newToken = await _jwtService.GenerateJwtTokenAsync(user);

            SetJwtCookie(newToken);

            return Ok(new 
            { 
                message = "Personal details updated successfully"
            });

        }

        private void SetJwtCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            };
            Response.Cookies.Append("JumiaAuthCookie", token, cookieOptions);
        }

        //extract the user ID from the token
        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    var result = await _authService.Asyncregister(registerDTO);

        //    if (!result.IsSuccess)
        //    {
        //        return BadRequest(result.Error);
        //    }
        //    return Ok(result.Value);

        //}

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    var result = await _authService.Asynclogin(loginDTO);

        //    if (!result.IsSuccess)
        //    {
        //        return Unauthorized(result.Error);
        //    }
        //    Response.Cookies.Append("jwt", result.Value.Token, new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.Lax,
        //        Expires = DateTime.UtcNow.AddMinutes(15)
        //    });
        //    return Ok(result.Value);
        //}

        //[HttpPost("logout")]
        //[Authorize]
        //public IActionResult Logout()
        //{
        //    Response.Cookies.Append("jwt", "", new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.Lax,
        //        Expires = DateTime.UtcNow.AddDays(-1)
        //    });

        //    return Ok(new { message = "Logged out successfully" });
        //}

        //[Authorize]
        //[HttpGet("me")]
        //public async Task<IActionResult> Me()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (userId == null)
        //    {
        //        return Unauthorized();
        //    }
        //    var email = User.FindFirstValue(ClaimTypes.Email);
        //    var userName = User.FindFirstValue(ClaimTypes.Name);
        //    var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        //    return Ok(new
        //    {
        //        UserId = Guid.Parse(userId),
        //        Email = email,
        //        Role = roles,
        //        Username = userName
        //    });


        //}
        //[HttpPost]
        //public async Task<IActionResult> CreateRole([FromBody] string roleName)
        //{
        //    var result  = await _authService.CreateRoleAsync(roleName);

        //    if (result.IsSuccess)
        //        return Ok(result.Value);

        //    return BadRequest(result.Error);
        //}
    }
}

