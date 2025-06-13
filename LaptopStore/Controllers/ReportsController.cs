using LaptopStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using LaptopStore.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace LaptopStore.Controllers
{
    [Authorize] // Đảm bảo chỉ người dùng được xác thực mới truy cập được
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public ReportsController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var viewModel = new ReportsViewModel();

            // 1. Tổng doanh thu
            // Đảm bảo TotalPrice trong Orders model là decimal hoặc float
            viewModel.TotalRevenue = await _context.Orders.SumAsync(o => ((decimal)o.TotalPrice));

            // 2. Tổng số đơn hàng
            viewModel.TotalOrders = await _context.Orders.CountAsync();

            // 3. Số đơn hàng còn chưa thanh toán
            viewModel.UnpaidOrdersCount = await _context.Orders.CountAsync(o => !o.Paid);

            // 4. Doanh thu nhân viên
            viewModel.EmployeeRevenues = await _context.Orders
                .Include(o => o.User) // Cần include User để truy cập thông tin nhân viên
                .GroupBy(o => o.User.UserName) // Nhóm theo tên nhân viên
                .Select(g => new EmployeeRevenueViewModel
                {
                    EmployeeName = g.Key,
                    TotalRevenue = g.Sum(o => ((decimal)o.TotalPrice))
                })
                .OrderByDescending(x => x.TotalRevenue) // Sắp xếp giảm dần theo doanh thu
                .ToListAsync();

            return View(viewModel);
        }
    }
}