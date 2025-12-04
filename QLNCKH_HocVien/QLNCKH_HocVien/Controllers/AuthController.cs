using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLNCKH_HocVien.Models;
using QLNCKH_HocVien.Services;

namespace QLNCKH_HocVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtService _jwtService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return Ok(new AuthResponse
                {
                    Success = false,
                    Message = "Tên đăng nhập hoặc mật khẩu không đúng"
                });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return Ok(new AuthResponse
                {
                    Success = false,
                    Message = "Tên đăng nhập hoặc mật khẩu không đúng"
                });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Đăng nhập thành công",
                Token = token,
                Username = user.UserName,
                Role = roles.FirstOrDefault(),
                HoTen = user.HoTen,
                IdSinhVien = user.IdSinhVien,
                IdGiaoVien = user.IdGiaoVien
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            // Kiểm tra username đã tồn tại
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
            {
                return Ok(new AuthResponse
                {
                    Success = false,
                    Message = "Tên đăng nhập đã tồn tại"
                });
            }

            // Kiểm tra email đã tồn tại
            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
            {
                return Ok(new AuthResponse
                {
                    Success = false,
                    Message = "Email đã được sử dụng"
                });
            }

            // Tạo user mới
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                HoTen = request.HoTen,
                IdSinhVien = request.IdSinhVien,
                IdGiaoVien = request.IdGiaoVien
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return Ok(new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                });
            }

            // Gán vai trò
            await _userManager.AddToRoleAsync(user, request.Role);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Đăng ký thành công",
                Token = token,
                Username = user.UserName,
                Role = request.Role,
                HoTen = user.HoTen,
                IdSinhVien = user.IdSinhVien,
                IdGiaoVien = user.IdGiaoVien
            });
        }

        // API kiểm tra token có còn valid không
        [HttpGet("validate")]
        public IActionResult ValidateToken()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return Ok(new AuthResponse
                {
                    Success = true,
                    Username = User.Identity.Name,
                    Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
                });
            }

            return Unauthorized();
        }
    }
}