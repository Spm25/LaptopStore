using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // Đảm bảo có dòng này
using LaptopStore.Data;
using LaptopStore.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using LaptopStore.ViewModels;
using Microsoft.AspNetCore.Http; // Đảm bảo có dòng này
using System.Globalization; // Cần thiết cho định dạng tiền tệ

namespace LaptopStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

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

            // --- Bổ sung logic để lấy và tính toán dữ liệu cho View Details ---
            var ordersCreatedByUser = await _context.Orders
                .Where(o => o.UserID == user.UserID) // Lọc theo UserID của người dùng hiện tại
                .Include(o => o.Customer) // Bao gồm thông tin Customer để hiển thị tên khách hàng
                .OrderByDescending(o => o.OrderDate) // Sắp xếp theo ngày đặt hàng mới nhất
                .ToListAsync();

            int totalOrdersCreated = ordersCreatedByUser.Count(); // Sửa lỗi .Count()
            float totalAmountOfCreatedOrders = ordersCreatedByUser.Sum(o => o.TotalPrice); // Sửa lỗi .Sum()
            float totalAmountUnpaidForCreatedOrders = ordersCreatedByUser.Where(o => !o.Paid).Sum(o => o.TotalPrice); // Sửa lỗi .Sum()

            ViewData["TotalOrdersCreated"] = totalOrdersCreated;
            ViewData["TotalAmountOfCreatedOrders"] = totalAmountOfCreatedOrders.ToString("N0", CultureInfo.InvariantCulture);
            ViewData["TotalAmountUnpaidForCreatedOrders"] = totalAmountUnpaidForCreatedOrders.ToString("N0", CultureInfo.InvariantCulture);
            ViewData["OrdersCreatedByUserList"] = ordersCreatedByUser; // Truyền danh sách đơn hàng đã tạo
            // --- Kết thúc phần bổ sung ---

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            PopulateRolesSelectList();
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Password,ConfirmPassword,Role")] UserCreateViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.UserName.Equals(userViewModel.UserName, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("UserName", "Tên người dùng đã tồn tại. Vui lòng chọn tên khác.");
                    PopulateRolesSelectList(userViewModel.Role);
                    return View(userViewModel);
                }

                var user = new User
                {
                    UserName = userViewModel.UserName,
                    Password = HashPassword(userViewModel.Password),
                    Role = userViewModel.Role
                };

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateRolesSelectList(userViewModel.Role);
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

            PopulateRolesSelectList(user.Role);

            var userViewModel = new UserCreateViewModel
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Role = user.Role
            };

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

            var existingUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserID == id);

            if (existingUser == null)
            {
                return NotFound();
            }

            // Logic bảo vệ tài khoản "admin" và tên người dùng (server-side)
            bool isProtectedAdminAccount = existingUser.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase);

            // 1. Bảo vệ UserName của tài khoản "admin"
            if (isProtectedAdminAccount && !userViewModel.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("UserName", "Không thể thay đổi tên đăng nhập của tài khoản 'admin'.");
                userViewModel.UserName = existingUser.UserName;
            }
            // Kiểm tra trùng lặp UserName mới (nếu không phải là tài khoản admin đang bị thay đổi)
            else if (!userViewModel.UserName.Equals(existingUser.UserName, StringComparison.OrdinalIgnoreCase) &&
                     await _context.Users.AnyAsync(u => u.UserName.Equals(userViewModel.UserName, StringComparison.OrdinalIgnoreCase) && u.UserID != id))
            {
                ModelState.AddModelError("UserName", "Tên người dùng mới đã tồn tại. Vui lòng chọn tên khác.");
            }


            // 2. Bảo vệ Role của tài khoản "admin"
            if (isProtectedAdminAccount)
            {
                userViewModel.Role = existingUser.Role;
            }


            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy lại existingUser với tracking để có thể update
                    var userToUpdate = await _context.Users.FindAsync(id);
                    if (userToUpdate == null) { return NotFound(); }

                    userToUpdate.UserName = userViewModel.UserName; // Cập nhật tên người dùng (nếu được phép)
                    userToUpdate.Role = userViewModel.Role;         // Cập nhật vai trò (nếu được phép)

                    if (!string.IsNullOrEmpty(userViewModel.Password))
                    {
                        userToUpdate.Password = HashPassword(userViewModel.Password);
                    }

                    _context.Update(userToUpdate);
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

            PopulateRolesSelectList(userViewModel.Role);
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