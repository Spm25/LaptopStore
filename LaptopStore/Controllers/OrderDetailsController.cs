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
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace LaptopStore.Controllers
{
    [Authorize]
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OrderDetails.Include(o => o.Order);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.OrderDetailID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }
        // Lấy tên các loại sản phẩm từ enum ProductType
        [HttpGet]
        public IActionResult GetProductsByType(string type)
        {
            if (!Enum.TryParse(type, out ProductType productType))
                return BadRequest("Loại sản phẩm không hợp lệ");

            List<SelectListItem> products = productType switch
            {
                ProductType.Laptop => _context.Laptops
                    .Where(l => !l.IsSold)
                    .Select(l => new SelectListItem { Value = l.LaptopID.ToString(), Text = l.Brand + " " + l.Model })
                    .ToList(),

                ProductType.RAM => _context.RAMs
                    .Select(r => new SelectListItem { Value = r.RAMID.ToString(), Text = $"{r.Capacity}GB {r.Type} {r.Speed}MHz" })
                    .ToList(),

                ProductType.LaptopCharger => _context.LaptopChargers
                    .Select(c => new SelectListItem { Value = c.ChargerID.ToString(), Text = $"{c.Wattage}W - {c.Connector}" })
                    .ToList(),

                ProductType.LaptopScreen => _context.LaptopScreens
                    .Select(s => new SelectListItem { Value = s.ScreenID.ToString(), Text = $"{s.Resolution} - {s.ScreenType}" })
                    .ToList(),

                ProductType.LaptopBattery => _context.LaptopBatteries
                    .Select(b => new SelectListItem { Value = b.BatteryID.ToString(), Text = $"{b.LaptopModel} - {b.Capacity}" })
                    .ToList(),

                ProductType.StorageDevice => _context.StorageDevices
                    .Select(sd => new SelectListItem { Value = sd.StorageID.ToString(), Text = $"{sd.Type} {sd.Capacity}" })
                    .ToList(),

                ProductType.Service => _context.Services
                    .Select(sv => new SelectListItem { Value = sv.ServiceID.ToString(), Text = sv.ServiceName })
                    .ToList(),

                _ => new List<SelectListItem>()
            };

            return Json(products);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            ViewBag.ProductType = new SelectList(Enum.GetValues(typeof(ProductType)));
            ViewBag.OrderID = new SelectList(_context.Orders, "OrderID", "OrderID");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDetailID,ProductID,ProductType,Quantity,UnitPrice,WarrantyPeriod,OrderID")] OrderDetail orderDetail)
        {
            
            _context.Add(orderDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
            //ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "OrderID", orderDetail.OrderID);
            //ViewData["ProductType"] = GetProductsByType();
            ViewBag.ProductType = new SelectList(Enum.GetValues(typeof(ProductType)));
            ViewBag.OrderID = new SelectList(_context.Orders, "OrderID", "OrderID");
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            //ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "OrderID", orderDetail.OrderID);
            //ViewData["ProductType"] = GetProductTypeSelectList();

            ViewBag.ProductType = new SelectList(Enum.GetValues(typeof(ProductType)));
            ViewBag.OrderID = new SelectList(_context.Orders, "OrderID", "OrderID");
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderDetailID,ProductID,ProductType,Quantity,UnitPrice,WarrantyPeriod,OrderID")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailID)
            {
                return NotFound();
            }

            
            try
            {
                _context.Update(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(orderDetail.OrderDetailID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            //ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "OrderID", orderDetail.OrderID);
            //ViewData["ProductType"] = GetProductTypeSelectList();
            ViewBag.ProductType = new SelectList(Enum.GetValues(typeof(ProductType)));
            ViewBag.OrderID = new SelectList(_context.Orders, "OrderID", "OrderID");
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.OrderDetailID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailID == id);
        }
    }
}
