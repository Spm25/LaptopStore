using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopStore.Data;
using LaptopStore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace LaptopStore.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Bước 1: Lấy thông tin cơ bản của khách hàng
            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);

            if (customer == null)
            {
                return NotFound();
            }

            // Bước 2: Truy vấn tường minh danh sách đơn hàng của khách hàng này từ bảng Orders
            var ordersForCustomer = await _context.Orders
                .Where(o => o.CustomerID == customer.CustomerID)
                .OrderByDescending(o => o.OrderDate) // Sắp xếp đơn hàng mới nhất lên đầu
                .ToListAsync();

            // Tính toán các thông tin tổng hợp từ danh sách ordersForCustomer
            int totalOrders = 0;
            float totalAmountPurchased = 0;
            float totalAmountOwed = 0;

            if (ordersForCustomer.Any()) // Kiểm tra nếu có đơn hàng
            {
                totalOrders = ordersForCustomer.Count();
                totalAmountPurchased = ordersForCustomer.Sum(o => o.TotalPrice);
                totalAmountOwed = ordersForCustomer.Where(o => !o.Paid).Sum(o => o.TotalPrice);
            }

            ViewData["TotalOrders"] = totalOrders;
            ViewData["TotalAmountPurchased"] = totalAmountPurchased.ToString("N0", CultureInfo.InvariantCulture);
            ViewData["TotalAmountOwed"] = totalAmountOwed.ToString("N0", CultureInfo.InvariantCulture);

            // Truyền danh sách đơn hàng sang View để hiển thị chi tiết (nếu cần)
            ViewData["CustomerOrdersList"] = ordersForCustomer;

            return View(customer); // Vẫn truyền model Customer cho các thông tin cơ bản
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,FullName,Phone,Email,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,FullName,Phone,Email,Address")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
    }
}
