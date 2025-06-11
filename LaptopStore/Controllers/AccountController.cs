using LaptopStore.Data;
using LaptopStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Security.Cryptography; // Thêm thư viện này
using System.Text; // Thêm thư viện này
using System.Threading.Tasks; // Đảm bảo có using này
using System.Linq; // Đảm bảo có using này

namespace LaptopStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Phương thức để băm mật khẩu sử dụng SHA256
        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty; // Hoặc ném một ArgumentNullException nếu mật khẩu không được để trống
            }

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Chuyển đổi mật khẩu thành mảng byte
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Chuyển đổi mảng byte thành chuỗi hex
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // "x2" để định dạng hex 2 ký tự
                }
                return builder.ToString();
            }
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(User userLogin) // Giả sử User có thuộc tính UserName và Password
        {
            if (userLogin == null || string.IsNullOrEmpty(userLogin.UserName) || string.IsNullOrEmpty(userLogin.Password))
            {
                ModelState.AddModelError("", "Tài khoản và mật khẩu không được để trống.");
                return View(userLogin);
            }

            if (!ModelState.IsValid) return View(userLogin);

            // Băm mật khẩu người dùng nhập vào để so sánh
            string hashedPasswordFromLogin = HashPassword(userLogin.Password);

            var user = _context.Users.FirstOrDefault(u => u.UserName == userLogin.UserName && u.Password == hashedPasswordFromLogin);

            if (user == null)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                return View(userLogin);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()), // Đảm bảo user.Role được chuyển đổi đúng sang chuỗi
                new Claim("UserID", user.UserID.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true, // Tùy chọn: giữ đăng nhập sau khi đóng trình duyệt
                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20) // Tùy chọn: Thời gian hết hạn cookie
            });

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        
    }
}