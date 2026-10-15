using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        public UsersController(SupermarketDbContext context)
        {
            _context = context;
        }

        // 1. GET /api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userDtos = await _context.Users.Select(u => new
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Role = u.Role,
                IsActive = u.IsActive
            }).ToListAsync();

            return Ok(userDtos);
        }

        // 2. GET /api/users/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng!" });
            }
            return Ok(new
            {
                user.Id,
                user.Username,
                user.FullName,
                user.Role,
                user.IsActive
            });
        }

        // 3. POST /api/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "Tên đăng nhập và mật khẩu không được trống!" });
            }

            var trimmedUsername = dto.Username.Trim();
            if (await _context.Users.AnyAsync(u => u.Username.ToLower() == trimmedUsername.ToLower()))
            {
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại trong hệ thống!" });
            }

            var newUser = new UserAccount
            {
                Username = trimmedUsername,
                Password = dto.Password.Trim(),
                FullName = string.IsNullOrWhiteSpace(dto.FullName) ? trimmedUsername : dto.FullName.Trim(),
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "Cashier" : dto.Role.Trim(),
                IsActive = true
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Tạo tài khoản thành công!", userId = newUser.Id });
        }

        // 4. PUT /api/users/{id}/toggle-lock
        [HttpPut("{id:int}/toggle-lock")]
        public async Task<IActionResult> ToggleLock(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng!" });
            }

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, isActive = user.IsActive, message = user.IsActive ? "Đã mở khóa tài khoản!" : "Đã khóa tài khoản!" });
        }

        // 5. POST /api/users/{id}/reset-password
        [HttpPost("{id:int}/reset-password")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng!" });
            }

            user.Password = "123456";
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Đã đặt lại mật khẩu về mặc định 123456!" });
        }
    }

    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = "Cashier";
    }
}
