﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopStore.Data;
using LaptopStore.Models;
using Microsoft.AspNetCore.Authorization;
using LaptopStore.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions; // Thêm dòng này

namespace LaptopStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf; // Khai báo INotyfService

        public OrdersController(ApplicationDbContext context, INotyfService notyf) // Thêm INotyfService vào constructor
        {
            _context = context;
            _notyf = notyf; // Gán
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orders.Include(o => o.Customer).Include(o => o.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _notyf.Error("ID đơn hàng không hợp lệ."); // Thêm thông báo lỗi
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .AsNoTracking() // Tối ưu hóa vì chỉ đọc dữ liệu
                .FirstOrDefaultAsync(m => m.OrderID == id);

            if (order == null)
            {
                _notyf.Error("Không tìm thấy đơn hàng này."); // Thêm thông báo lỗi
                return NotFound();
            }

            var viewModel = new OrderDetailsPageViewModel
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                Paid = order.Paid,
                OrderTotalPrice = order.TotalPrice
            };

            if (order.Customer != null)
            {
                viewModel.CustomerInfo = new CustomerViewModel
                {
                    CustomerID = order.Customer.CustomerID,
                    FullName = order.Customer.FullName,
                    Phone = order.Customer.Phone,
                    Email = order.Customer.Email,
                    Address = order.Customer.Address
                };
            }

            if (order.User != null)
            {
                viewModel.UserInfo = new UserViewModel
                {
                    UserID = order.User.UserID,
                    UserName = order.User.UserName
                };
            }

            if (order.OrderDetails != null)
            {
                foreach (var detail in order.OrderDetails)
                {
                    var orderDetailVM = new OrderDetailViewModel
                    {
                        OrderDetailID = detail.OrderDetailID,
                        ProductID = detail.ProductID,
                        ProductType = detail.ProductType,
                        Quantity = detail.Quantity,
                        UnitPrice = detail.UnitPrice,
                        WarrantyPeriod = detail.WarrantyPeriod ?? "Không có"
                    };

                    // Lấy thông tin chi tiết sản phẩm
                    switch (detail.ProductType)
                    {
                        case ProductType.Laptop:
                            var laptop = await _context.Laptops.FindAsync(detail.ProductID);
                            if (laptop != null)
                            {
                                orderDetailVM.ProductName = $"Laptop {laptop.Brand} {laptop.Model}";
                                orderDetailVM.ProductDescription = $"CPU: {laptop.CPU}, RAM: {laptop.RAM}, Lưu trữ: {laptop.Storage}, GPU: {laptop.GPU}";
                            }
                            break;
                        case ProductType.LaptopCharger:
                            var charger = await _context.LaptopChargers.FindAsync(detail.ProductID);
                            if (charger != null)
                            {
                                orderDetailVM.ProductName = $"Sạc Laptop {charger.Wattage}W";
                                orderDetailVM.ProductDescription = $"Đầu nối: {charger.Connector}, Chất lượng: {charger.Quality}";
                            }
                            break;
                        case ProductType.RAM:
                            var ram = await _context.RAMs.FindAsync(detail.ProductID);
                            if (ram != null)
                            {
                                orderDetailVM.ProductName = $"RAM {ram.Capacity}GB {ram.Type}";
                                orderDetailVM.ProductDescription = $"Tốc độ: {ram.Speed}MHz, Chất lượng: {ram.Quality}";
                            }
                            break;
                        case ProductType.LaptopScreen:
                            var screen = await _context.LaptopScreens.FindAsync(detail.ProductID);
                            if (screen != null)
                            {
                                orderDetailVM.ProductName = $"Màn hình {screen.Resolution}";
                                orderDetailVM.ProductDescription = $"Loại: {screen.ScreenType}, Tình trạng: {screen.UsedStatus}, Đã sửa: {(screen.Repaired == 1 ? "Đã sửa" : "Chưa sửa")}";
                            }
                            break;
                        case ProductType.LaptopBattery:
                            var battery = await _context.LaptopBatteries.FindAsync(detail.ProductID);
                            if (battery != null)
                            {
                                orderDetailVM.ProductName = $"Pin cho {battery.LaptopModel}";
                                orderDetailVM.ProductDescription = $"Dung lượng: {battery.Capacity}, Loại: {battery.Type}, Tình trạng: {(battery.UsedStatus == 0 ? "Mới" : "Đã dùng")}";
                            }
                            break;
                        case ProductType.StorageDevice:
                            var storage = await _context.StorageDevices.FindAsync(detail.ProductID);
                            if (storage != null)
                            {
                                orderDetailVM.ProductName = $"Ổ cứng {storage.Type} {storage.Capacity}";
                                orderDetailVM.ProductDescription = $"Chất lượng: {storage.Quality}";
                            }
                            break;
                        case ProductType.Service:
                            var service = await _context.Services.FindAsync(detail.ProductID);
                            if (service != null)
                            {
                                orderDetailVM.ProductName = service.ServiceName;
                                orderDetailVM.ProductDescription = service.Description;
                            }
                            break;
                        default:
                            orderDetailVM.ProductName = "Sản phẩm/Dịch vụ không xác định";
                            break;
                    }
                    viewModel.OrderDetails.Add(orderDetailVM);
                }
            }

            return View(viewModel);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = _context.Customers
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerID.ToString(),
                    Text = $"{c.FullName} - {c.Phone}"
                }).ToList();

            var username = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            var order = new Order();
            if (user != null)
            {
                order.UserID = user.UserID;
                ViewData["UserID"] = user.UserID;
                ViewData["UserFullName"] = user.UserName;
            }

            return View(order);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,OrderDate,Paid,CustomerID,UserID")] Order order)
        {
            try
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                _notyf.Success("Tạo đơn hàng thành công!"); // Thêm thông báo thành công
                return RedirectToAction(nameof(Details), new { id = order.OrderID });
            }
            catch (Exception ex)
            {
                _notyf.Error("Có lỗi xảy ra khi tạo đơn hàng: " + ex.Message); // Thêm thông báo lỗi
                // Bạn có thể thêm ModelState.AddModelError nếu muốn hiển thị lỗi trên form
                // ModelState.AddModelError("", "Có lỗi xảy ra khi tạo đơn hàng: " + ex.Message);
            }

            // Nếu có lỗi, load lại ViewData cho dropdown và trả về view
            ViewData["CustomerID"] = _context.Customers
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerID.ToString(),
                    Text = $"{c.FullName} - {c.Phone}"
                }).ToList();

            var username = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                ViewData["UserID"] = user.UserID;
                ViewData["UserFullName"] = user.UserName;
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _notyf.Error("ID đơn hàng không hợp lệ."); // Thêm thông báo lỗi
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                _notyf.Error("Không tìm thấy đơn hàng để chỉnh sửa."); // Thêm thông báo lỗi
                return NotFound();
            }

            // Cập nhật ViewData cho dropdown
            ViewData["CustomerID"] = _context.Customers
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerID.ToString(),
                    Text = $"{c.FullName} - {c.Phone}"
                }).ToList();

            ViewData["UserID"] = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.UserID.ToString(),
                    Text = u.UserName
                }).ToList();

            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,OrderDate,Paid,CustomerID,UserID")] Order order)
        {
            if (id != order.OrderID)
            {
                _notyf.Error("ID đơn hàng không khớp."); // Thêm thông báo lỗi
                return NotFound();
            }

            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
                _notyf.Success("Cập nhật đơn hàng thành công!"); // Thêm thông báo thành công
                await RecalculateTotalPrice(order.OrderID);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.OrderID))
                {
                    _notyf.Error("Không tìm thấy đơn hàng này trong quá trình cập nhật."); // Thêm thông báo lỗi
                    return NotFound();
                }
                else
                {
                    _notyf.Error("Có lỗi xảy ra khi cập nhật do xung đột dữ liệu. Vui lòng thử lại."); // Thêm thông báo lỗi
                    throw;
                }
            }
            catch (Exception ex)
            {
                _notyf.Error("Có lỗi xảy ra khi cập nhật đơn hàng: " + ex.Message); // Thêm thông báo lỗi
            }
            return RedirectToAction(nameof(Index));

            // Dòng này có thể bị lỗi vì RedirectToAction đã được gọi trước đó trong khối try-catch.
            // Nếu bạn muốn hiển thị lại form khi có lỗi, hãy di chuyển code này vào catch block và bỏ RedirectToAction.
            // ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", order.CustomerID);
            // ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", order.UserID);
            // return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _notyf.Error("ID đơn hàng không hợp lệ."); // Thêm thông báo lỗi
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                _notyf.Error("Không tìm thấy đơn hàng để xóa."); // Thêm thông báo lỗi
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                _notyf.Information("Đã xóa đơn hàng thành công."); // Thêm thông báo thành công
            }
            else
            {
                _notyf.Error("Không tìm thấy đơn hàng để xóa."); // Thêm thông báo lỗi
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }

        // POST: Orders/RecalculateTotalPrice/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecalculateTotalPrice(int orderId) // Đổi tên tham số cho rõ ràng
        {
            var order = await _context.Orders
                                    .Include(o => o.OrderDetails) // Cần OrderDetails để tính tổng
                                    .FirstOrDefaultAsync(o => o.OrderID == orderId);

            if (order == null)
            {
                _notyf.Error("Không tìm thấy đơn hàng để tính toán lại tổng tiền."); // Thêm thông báo lỗi
                return NotFound(); // Hoặc RedirectToAction("Index")
            }

            float newTotalPrice = 0;
            if (order.OrderDetails != null && order.OrderDetails.Any())
            {
                newTotalPrice = order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice);
            }

            order.TotalPrice = newTotalPrice;

            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
                _notyf.Success("Đã tính lại tổng tiền đơn hàng thành công!"); // Thêm thông báo thành công
            }
            catch (DbUpdateConcurrencyException)
            {
                _notyf.Error("Lỗi khi cập nhật tổng tiền do xung đột dữ liệu. Vui lòng thử lại."); // Thêm thông báo lỗi
                if (!OrderExists(order.OrderID))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Hoặc xử lý theo cách khác
                }
            }
            catch (Exception ex)
            {
                _notyf.Error($"Đã xảy ra lỗi không mong muốn khi tính lại tổng tiền: {ex.Message}"); // Thêm thông báo lỗi
                // Nên log lỗi chi tiết (ex)
            }

            return RedirectToAction(nameof(Details), new { id = orderId });
        }
    }
}