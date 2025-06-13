using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopStore.Data;
using LaptopStore.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection; // Cần thiết cho DisplayAttribute
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using LaptopStore.ViewModels;
using Microsoft.AspNetCore.Http; // Đảm bảo đã thêm HttpContextAccessor

namespace LaptopStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor; // Khai báo IHttpContextAccessor

        // Constructor đã được cập nhật để inject IHttpContextAccessor
        public UsersController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Phương thức để băm mật khẩu sử dụng SHA256
        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            // Tạo SelectList từ enum UserRole với Display Name
            var roles = Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Select(r => new SelectListItem
                {
                    Value = r.ToString(),
                    Text = r.GetType()
                             .GetMember(r.ToString())
                             .First()
                             .GetCustomAttribute<DisplayAttribute>()?
                             .Name ?? r.ToString()
                }).ToList();

            ViewBag.Roles = new SelectList(roles, "Value", "Text");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Password,ConfirmPassword,Role")] UserCreateViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem UserName đã tồn tại chưa
                if (await _context.Users.AnyAsync(u => u.UserName.Equals(userViewModel.UserName, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("UserName", "Tên người dùng đã tồn tại. Vui lòng chọn tên khác.");
                    // Cần load lại ViewBag.Roles nếu có lỗi và trả về View
                    PopulateRolesSelectList(userViewModel.Role); // Gọi hàm để populate lại SelectList
                    return View(userViewModel);
                }

                var user = new User
                {
                    UserName = userViewModel.UserName,
                    Password = HashPassword(userViewModel.Password), // Băm mật khẩu
                    Role = userViewModel.Role
                };

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateRolesSelectList(userViewModel.Role); // Gọi hàm để populate lại SelectList
            return View(userViewModel);
        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Populate ViewBag.Roles
            PopulateRolesSelectList(user.Role);

            // Chuyển đổi từ User model sang UserCreateViewModel để điền vào form Edit
            var userViewModel = new UserCreateViewModel
            {
                UserID = user.UserID, // Gán UserID vào ViewModel
                UserName = user.UserName,
                // Password và ConfirmPassword sẽ để trống để người dùng nhập mới nếu muốn thay đổi
                Role = user.Role
            };

            // Truyền thông tin về tài khoản admin và người dùng đang đăng nhập cho View
            // (Đã loại bỏ ViewData["IsProtectedAdmin"] và ViewData["CurrentLoggedInUserName"]
            //  vì View có thể tự tính toán từ Model và HttpContextAccessor)
            return View(userViewModel);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserName,Password,ConfirmPassword,Role")] UserCreateViewModel userViewModel)
        {
            if (id != userViewModel.UserID)
            {
                return NotFound();
            }

            // Lấy thông tin người dùng hiện tại từ database (AsNoTracking để tránh conflict nếu có)
            var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserID == id);

            if (existingUser == null)
            {
                return NotFound();
            }

            // Lấy tên người dùng đang đăng nhập
            var currentLoggedInUserName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            // Logic bảo vệ tài khoản "admin" và tên người dùng (server-side)
            bool isProtectedAdminAccount = existingUser.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase);

            // 1. Bảo vệ UserName của tài khoản "admin"
            if (isProtectedAdminAccount && !userViewModel.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("UserName", "Không thể thay đổi tên đăng nhập của tài khoản 'admin'.");
                // Giữ lại tên cũ trong ViewModel để hiển thị lại form với lỗi
                userViewModel.UserName = existingUser.UserName;
            }
            // Kiểm tra trùng lặp UserName mới (nếu không phải là tài khoản admin đang bị thay đổi)
            else if (!userViewModel.UserName.Equals(existingUser.UserName, StringComparison.OrdinalIgnoreCase) &&
                     await _context.Users.AnyAsync(u => u.UserName.Equals(userViewModel.UserName, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("UserName", "Tên người dùng mới đã tồn tại. Vui lòng chọn tên khác.");
            }


            // 2. Bảo vệ Role của tài khoản "admin"
            if (isProtectedAdminAccount)
            {
                // Nếu là tài khoản admin, giữ nguyên vai trò cũ từ DB, không cho phép thay đổi từ form
                userViewModel.Role = existingUser.Role; // Đảm bảo ViewModel có giá trị Role đúng khi trả về View
                // Không cần gán existingUser.Role = userViewModel.Role; ở đây
            }
            // Không cần xử lý trường hợp người dùng đang chỉnh sửa chính họ ở đây cho Role,
            // vì logic Role đã được xử lý bằng isProtectedAdminAccount.


            if (ModelState.IsValid) // Kiểm tra lại ModelState sau khi thêm lỗi tùy chỉnh
            {
                try
                {
                    // Tạo một đối tượng User mới để cập nhật, hoặc gắn existingUser và thay đổi trạng thái
                    // Phương pháp này an toàn hơn khi dùng AsNoTracking() ban đầu
                    existingUser.UserName = userViewModel.UserName; // Cập nhật tên người dùng (nếu được phép)
                    existingUser.Role = userViewModel.Role;         // Cập nhật vai trò (nếu được phép)

                    // Cập nhật mật khẩu nếu có nhập mới và hợp lệ
                    if (!string.IsNullOrEmpty(userViewModel.Password))
                    {
                        existingUser.Password = HashPassword(userViewModel.Password);
                    }

                    _context.Update(existingUser); // Đánh dấu entity là Modified
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, load lại ViewBag.Roles và trả về View
            PopulateRolesSelectList(userViewModel.Role); // Gọi hàm để populate lại SelectList

            return View(userViewModel);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            // Logic bảo vệ tài khoản "admin" không cho phép xóa
            if (user.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                // Có thể thêm NotyfService.Error hoặc TempData ở đây
                TempData["ErrorMessage"] = "Không thể xóa tài khoản 'admin' gốc.";
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Kiểm tra lại logic bảo vệ tài khoản "admin" trước khi xóa
            if (user.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản 'admin' gốc.";
                return RedirectToAction(nameof(Index));
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper method để tạo SelectList cho các vai trò
        private void PopulateRolesSelectList(UserRole? selectedRole = null)
        {
            var roles = Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Select(r => new SelectListItem
                {
                    Value = r.ToString(),
                    Text = r.GetType()
                             .GetMember(r.ToString())
                             .First()
                             .GetCustomAttribute<DisplayAttribute>()?
                             .Name ?? r.ToString()
                }).ToList();

            ViewBag.Roles = new SelectList(roles, "Value", "Text", selectedRole?.ToString());
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}