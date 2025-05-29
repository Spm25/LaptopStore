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
using AspNetCoreHero.ToastNotification.Abstractions;

namespace LaptopStore.Controllers
{
    [Authorize]
    public class LaptopsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }
        public LaptopsController(ApplicationDbContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Laptops
        public async Task<IActionResult> Index()
        {
            return View(await _context.Laptops.ToListAsync());
        }

        // GET: Laptops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptops
                .FirstOrDefaultAsync(m => m.LaptopID == id);
            if (laptop == null)
            {
                _notifyService.Error("Không tìm thấy Laptop này.");
                return NotFound();
            }

            return View(laptop);
        }

        // GET: Laptops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Laptops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LaptopID,Brand,Model,SerialNumber,CPU,RAM,Storage,GPU,ImportPrice,SellingPrice,Description,ImageURL,ScreenSize,OperatingSystem,BatteryHealth,IsSold")] Laptop laptop)
        {
            if (!string.IsNullOrEmpty(laptop.SerialNumber))
            {
                bool serialExists = await _context.Laptops.AnyAsync(l => l.SerialNumber == laptop.SerialNumber);
                if (serialExists)
                {
                    ModelState.AddModelError("SerialNumber", "Số serial này đã tồn tại trong hệ thống.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(laptop);
                await _context.SaveChangesAsync();
                _notifyService.Success("Đã thêm mới Laptop thành công!");
                return RedirectToAction(nameof(Index));
            }
            _notifyService.Error("Thêm mới Laptop thất bại. Vui lòng kiểm tra lại thông tin.");
            return View(laptop);
        }

        // GET: Laptops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _notifyService.Error("ID Laptop không hợp lệ.");
                return NotFound();
            }

            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop == null)
            {
                _notifyService.Error("Không tìm thấy Laptop để chỉnh sửa.");
                return NotFound();
            }
            return View(laptop);
        }

        // POST: Laptops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LaptopID,Brand,Model,SerialNumber,CPU,RAM,Storage,GPU,ImportPrice,SellingPrice,Description,ImageURL,ScreenSize,OperatingSystem,BatteryHealth,IsSold")] Laptop laptop)
        {
            if (id != laptop.LaptopID)
            {
                _notifyService.Error("ID Laptop không khớp.");
                return NotFound();
            }

            if (!string.IsNullOrEmpty(laptop.SerialNumber))
            {
                bool serialExistsForOtherLaptop = await _context.Laptops.AnyAsync(l => l.SerialNumber == laptop.SerialNumber && l.LaptopID != laptop.LaptopID);
                if (serialExistsForOtherLaptop)
                {
                    ModelState.AddModelError("SerialNumber", "Số serial này đã được sử dụng cho một laptop khác.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laptop);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Cập nhật Laptop thành công!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaptopExists(laptop.LaptopID))
                    {
                        _notifyService.Error("Không tìm thấy Laptop này trong quá trình cập nhật.");
                        return NotFound();
                    }
                    else
                    {
                        _notifyService.Error("Có lỗi xảy ra khi cập nhật do xung đột dữ liệu. Vui lòng thử lại.");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            _notifyService.Error("Cập nhật Laptop thất bại. Vui lòng kiểm tra lại thông tin.");
            return View(laptop);
        }

        // GET: Laptops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _notifyService.Error("ID Laptop không hợp lệ.");
                return NotFound();
            }

            var laptop = await _context.Laptops
                .FirstOrDefaultAsync(m => m.LaptopID == id);
            if (laptop == null)
            {
                _notifyService.Error("Không tìm thấy Laptop để xóa.");
                return NotFound();
            }

            return View(laptop);
        }

        // POST: Laptops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var laptop = await _context.Laptops.FindAsync(id);
            if (laptop != null)
            {
                _context.Laptops.Remove(laptop);
                _notifyService.Information("Đã xóa Laptop thành công.");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaptopExists(int id)
        {
            return _context.Laptops.Any(e => e.LaptopID == id);
        }
    }
}
