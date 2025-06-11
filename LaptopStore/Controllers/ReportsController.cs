using LaptopStore.Data;
using LaptopStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Cần cho DisplayAttribute
using System.Globalization;
using System.Linq;
using System.Reflection; // Cần cho GetCustomAttribute
using System.Threading.Tasks;

namespace LaptopStore.Controllers
{
    /*[Authorize] */// Yêu cầu đăng nhập để xem báo cáo
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        public IActionResult Index()
        {
            return View();
        }

        // API để lấy tên hiển thị của ProductType (từ DisplayAttribute)
        private string GetProductTypeDisplayName(ProductType productType)
        {
            return typeof(ProductType)
                .GetMember(productType.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName() ?? productType.ToString();
        }


        #region API Endpoints cho Biểu đồ

        // --- Doanh thu theo thời gian (ví dụ: 12 tháng gần nhất) ---
        [HttpGet]
        public async Task<JsonResult> GetRevenueOverTimeData(int monthsToShow = 12)
        {
            var today = DateTime.Today;
            // Bắt đầu từ ngày đầu tiên của tháng (monthsToShow - 1) tháng trước
            var startDate = new DateTime(today.Year, today.Month, 1).AddMonths(-(monthsToShow - 1));

            var monthlyRevenue = await _context.Orders
                .Where(o => o.Paid && o.OrderDate >= startDate && o.OrderDate <= today)
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(o => o.TotalPrice)
                })
                .OrderBy(r => r.Year).ThenBy(r => r.Month)
                .ToListAsync();

            // Tạo đủ các tháng trong khoảng thời gian, kể cả tháng không có doanh thu
            var reportData = new List<dynamic>();
            for (int i = 0; i < monthsToShow; i++)
            {
                var currentDate = startDate.AddMonths(i);
                var revenueEntry = monthlyRevenue.FirstOrDefault(r => r.Year == currentDate.Year && r.Month == currentDate.Month);
                reportData.Add(new
                {
                    MonthYear = currentDate.ToString("MMM yyyy", CultureInfo.InvariantCulture),
                    Revenue = revenueEntry?.TotalRevenue ?? 0
                });
            }

            var labels = reportData.Select(r => r.MonthYear).ToList();
            var data = reportData.Select(r => r.Revenue).ToList();

            return Json(new { labels, data });
        }

        // --- Tỷ lệ doanh thu theo loại sản phẩm ---
        [HttpGet]
        public async Task<JsonResult> GetRevenueByProductTypeData()
        {
            var revenueByProductType = await _context.OrderDetails
                .Include(od => od.Order) // Join với Order để kiểm tra Paid
                .Where(od => od.Order.Paid)
                .GroupBy(od => od.ProductType)
                .Select(g => new
                {
                    ProductTypeEnum = g.Key, // Giữ lại dạng enum để lấy DisplayName
                    TotalRevenue = g.Sum(od => od.Quantity * od.UnitPrice)
                })
                .OrderByDescending(r => r.TotalRevenue)
                .ToListAsync();

            var labels = revenueByProductType.Select(r => GetProductTypeDisplayName(r.ProductTypeEnum)).ToList();
            var data = revenueByProductType.Select(r => r.TotalRevenue).ToList();

            return Json(new { labels, data });
        }

        // --- Doanh thu theo Nhân viên ---
        // Trong ReportsController.cs
        [HttpGet]
        public async Task<JsonResult> GetRevenueByUserData()
        {
            var revenueByUser = await _context.Orders
                .Where(o => o.Paid && o.User != null) // Quan trọng: kiểm tra o.User không null
                .Include(o => o.User) // Đã có sẵn, tốt!
                .GroupBy(o => o.User.UserName)
                .Select(g => new
                {
                    UserName = g.Key ?? "Không xác định",
                    TotalRevenue = g.Sum(o => o.TotalPrice)
                })
                .OrderByDescending(r => r.TotalRevenue)
                .ToListAsync();

            var labels = revenueByUser.Select(r => r.UserName).ToList();
            var data = revenueByUser.Select(r => r.TotalRevenue).ToList();

            return Json(new { labels, data });
        }


        // --- Top 5 Loại sản phẩm bán chạy (theo số lượng) ---
        [HttpGet]
        public async Task<JsonResult> GetTopProductTypesByQuantity(int count = 5)
        {
            var topProductTypes = await _context.OrderDetails
                .GroupBy(od => od.ProductType)
                .Select(g => new
                {
                    ProductTypeEnum = g.Key,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(pt => pt.TotalQuantity)
                .Take(count)
                .ToListAsync();

            var labels = topProductTypes.Select(pt => GetProductTypeDisplayName(pt.ProductTypeEnum)).ToList();
            var data = topProductTypes.Select(pt => pt.TotalQuantity).ToList();

            return Json(new { labels, data });
        }

        #endregion
    }
}