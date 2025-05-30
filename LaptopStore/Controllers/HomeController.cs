using LaptopStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Đảm bảo có using này

namespace LaptopStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Tính doanh thu tháng hiện tại
            var currentDate = DateTime.Now;
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            decimal monthlyRevenue = (decimal)await _context.Orders
                .Where(o => o.OrderDate >= firstDayOfMonth && o.OrderDate <= lastDayOfMonth)
                .SumAsync(o => o.TotalPrice); // Giả định TotalPrice là decimal, nếu là nullable decimal hoặc kiểu khác cần ép kiểu (decimal)

            ViewBag.MonthlyRevenue = monthlyRevenue;

            // 2. Tính số lượng tồn kho cho từng loại sản phẩm
            // Laptop: Đếm số lượng không bị đánh dấu IsSold = true
            int laptopsInStock = await _context.Laptops.CountAsync(l => !l.IsSold);
            ViewBag.LaptopsInStock = laptopsInStock;

            // RAM: Tổng Quantity của tất cả các record RAM
            int ramsInStock = await _context.RAMs.SumAsync(r => r.Quantity);
            ViewBag.RAMsInStock = ramsInStock;

            // Storage Devices: Tổng Quantity
            int storageInStock = await _context.StorageDevices.SumAsync(s => s.Quantity);
            ViewBag.StorageDevicesInStock = storageInStock;

            // Laptop Screens: Tổng Quantity
            int screensInStock = await _context.LaptopScreens.SumAsync(s => s.Quantity);
            ViewBag.LaptopScreensInStock = screensInStock;

            // Laptop Batteries: Tổng Quantity
            int batteriesInStock = await _context.LaptopBatteries.SumAsync(b => b.Quantity);
            ViewBag.LaptopBatteriesInStock = batteriesInStock;

            // Laptop Chargers: Tổng Quantity
            int chargersInStock = await _context.LaptopChargers.SumAsync(c => c.Quantity);
            ViewBag.LaptopChargersInStock = chargersInStock;

            // (Tùy chọn) Tổng tồn kho tất cả sản phẩm nếu vẫn muốn dùng đâu đó
            // int totalProductsInStock = laptopsInStock + ramsInStock + storageInStock + screensInStock + batteriesInStock + chargersInStock;
            // ViewBag.TotalProductsInStock = totalProductsInStock;


            // 3. Số đơn hàng trong tháng
            int monthlyOrders = await _context.Orders
                .CountAsync(o => o.OrderDate >= firstDayOfMonth && o.OrderDate <= lastDayOfMonth);
            ViewBag.MonthlyOrders = monthlyOrders;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}