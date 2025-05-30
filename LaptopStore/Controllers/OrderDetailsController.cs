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

            var products = productType switch
            {
                ProductType.Laptop => _context.Laptops
                    .Where(l => !l.IsSold)
                    .Select(l => new
                    {
                        Value = l.LaptopID.ToString(),
                        Text = $"{l.Brand} {l.Model} (CPU: {l.CPU}, RAM: {l.RAM})",
                        Quantity = 1 // Mỗi laptop là 1 sản phẩm duy nhất
                    }),

                ProductType.RAM => _context.RAMs
                    .Where(r => r.Quantity > 0)
                    .Select(r => new
                    {
                        Value = r.RAMID.ToString(),
                        Text = $"{r.Capacity}GB {r.Type} (Còn: {r.Quantity})",
                        Quantity = r.Quantity
                    }),

                ProductType.LaptopCharger => _context.LaptopChargers
                    .Where(c => c.Quantity > 0)
                    .Select(c => new
                    {
                        Value = c.ChargerID.ToString(),
                        Text = $"{c.Wattage}W {c.Connector} (Còn: {c.Quantity})",
                        Quantity = c.Quantity
                    }),

                ProductType.LaptopScreen => _context.LaptopScreens
                    .Where(s => s.Quantity > 0)
                    .Select(s => new
                    {
                        Value = s.ScreenID.ToString(),
                        Text = $"{s.Resolution} {s.ScreenType} (Còn: {s.Quantity})",
                        Quantity = s.Quantity
                    }),

                ProductType.LaptopBattery => _context.LaptopBatteries
                    .Where(b => b.Quantity > 0)
                    .Select(b => new
                    {
                        Value = b.BatteryID.ToString(),
                        Text = $"{b.LaptopModel} {b.Capacity} (Còn: {b.Quantity})",
                        Quantity = b.Quantity
                    }),

                ProductType.StorageDevice => _context.StorageDevices
                    .Where(s => s.Quantity > 0)
                    .Select(s => new
                    {
                        Value = s.StorageID.ToString(),
                        Text = $"{s.Type} {s.Capacity} (Còn: {s.Quantity})",
                        Quantity = s.Quantity
                    }),

                ProductType.Service => _context.Services
                    .Select(s => new
                    {
                        Value = s.ServiceID.ToString(),
                        Text = s.ServiceName,
                        Quantity = 100 // Dịch vụ không giới hạn số lượng
                    }),

                _ => Enumerable.Empty<object>()
            };

            return Json(products.ToList());
        }

        // GET: OrderDetails/Create
        // GET: OrderDetails/Create
        public IActionResult Create(int? orderId)
        {
            if (orderId == null)
            {
                TempData["ErrorMessage"] = "Cần có mã đơn hàng để thêm chi tiết sản phẩm.";
                return RedirectToAction("Index", "Orders"); // Hoặc một trang lỗi/thông báo phù hợp
            }

            var order = _context.Orders.Find(orderId.Value); // Kiểm tra OrderID có hợp lệ không
            if (order == null)
            {
                TempData["ErrorMessage"] = "Đơn hàng không tồn tại.";
                return RedirectToAction("Index", "Orders");
            }

            var orderDetail = new OrderDetail
            {
                OrderID = orderId.Value // Gán OrderID cho đối tượng OrderDetail mới
            };

            // Truyền OrderID sang View để dùng cho các link "Hủy" hoặc hiển thị thông tin
            ViewData["CurrentOrderID"] = orderId.Value;

            // Tạo SelectList từ enum với Display Name
            var productTypes = Enum.GetValues(typeof(ProductType))
                .Cast<ProductType>()
                .Select(pt => new SelectListItem
                {
                    Value = pt.ToString(),
                    Text = pt.GetType()
                           .GetMember(pt.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()?
                           .Name ?? pt.ToString()
                }).ToList();

            ViewBag.ProductType = new SelectList(productTypes, "Value", "Text");
            ViewBag.OrderID = new SelectList(_context.Orders, "OrderID", "OrderID");
            return View(orderDetail);
        }

        
        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDetailID,ProductID,ProductType,Quantity,UnitPrice,WarrantyPeriod,OrderID")] OrderDetail orderDetail)
        {
            
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // 1. Thêm OrderDetail mới
                        _context.Add(orderDetail);
                        await _context.SaveChangesAsync();

                        // 2. Cập nhật số lượng sản phẩm
                        await UpdateProductQuantity(orderDetail.ProductType, orderDetail.ProductID, orderDetail.Quantity);

                        // 3. Cập nhật tổng tiền đơn hàng
                        await UpdateOrderTotalPrice(orderDetail.OrderID);

                        await transaction.CommitAsync();
                        return RedirectToAction("Details", "Orders", new { id = orderDetail.OrderID });
                }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "Có lỗi xảy ra khi tạo chi tiết đơn hàng: " + ex.Message);
                    }
                }
            

            ViewBag.ProductType = new SelectList(Enum.GetValues(typeof(ProductType)));
            ViewBag.OrderID = new SelectList(_context.Orders, "OrderID", "OrderID");
            return RedirectToAction("Details", "Orders", new { id = orderDetail.OrderID });
        }

        private async Task UpdateProductQuantity(ProductType productType, int productId, int quantity, bool isReturn = false)
        {
            switch (productType)
            {
                case ProductType.Laptop:
                    var laptop = await _context.Laptops.FindAsync(productId);
                    if (laptop != null)
                    {
                        laptop.IsSold = !isReturn; // true khi trừ, false khi hoàn trả
                        _context.Update(laptop);
                    }
                    break;

                case ProductType.RAM:
                    var ram = await _context.RAMs.FindAsync(productId);
                    if (ram != null)
                    {
                        ram.Quantity += isReturn ? quantity : -quantity;
                        _context.Update(ram);
                    }
                    break;

                case ProductType.LaptopCharger:
                    var charger = await _context.LaptopChargers.FindAsync(productId);
                    if (charger != null)
                    {
                        charger.Quantity += isReturn ? quantity : -quantity;
                        _context.Update(charger);
                    }
                    break;

                case ProductType.LaptopScreen:
                    var screen = await _context.LaptopScreens.FindAsync(productId);
                    if (screen != null)
                    {
                        screen.Quantity += isReturn ? quantity : -quantity;
                        _context.Update(screen);
                    }
                    break;

                case ProductType.LaptopBattery:
                    var battery = await _context.LaptopBatteries.FindAsync(productId);
                    if (battery != null)
                    {
                        battery.Quantity += isReturn ? quantity : -quantity;
                        _context.Update(battery);
                    }
                    break;

                case ProductType.StorageDevice:
                    var storage = await _context.StorageDevices.FindAsync(productId);
                    if (storage != null)
                    {
                        storage.Quantity += isReturn ? quantity : -quantity;
                        _context.Update(storage);
                    }
                    break;

                case ProductType.Service:
                    // Dịch vụ không cần cập nhật số lượng
                    break;

                default:
                    throw new ArgumentException("Loại sản phẩm không hợp lệ");
            }

            await _context.SaveChangesAsync();
        }

        private async Task UpdateOrderTotalPrice(int orderId)
        {
            // Lấy tất cả OrderDetail thuộc về Order này
            var orderDetails = await _context.OrderDetails
                .Where(od => od.OrderID == orderId)
                .ToListAsync();

            // Tính tổng giá trị đơn hàng
            float totalPrice = orderDetails.Sum(od => od.Quantity * od.UnitPrice);

            // Cập nhật vào Order
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.TotalPrice = totalPrice;
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
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

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Lấy thông tin OrderDetail cũ
                    var oldOrderDetail = await _context.OrderDetails.AsNoTracking()
                        .FirstOrDefaultAsync(od => od.OrderDetailID == id);

                    if (oldOrderDetail != null)
                    {
                        // Hoàn trả số lượng sản phẩm cũ (isReturn = true)
                        await UpdateProductQuantity(oldOrderDetail.ProductType, oldOrderDetail.ProductID, oldOrderDetail.Quantity, true);
                    }

                    // Cập nhật OrderDetail mới
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();

                    // Trừ số lượng sản phẩm mới (isReturn = false)
                    await UpdateProductQuantity(orderDetail.ProductType, orderDetail.ProductID, orderDetail.Quantity, false);

                    // Cập nhật tổng tiền đơn hàng
                    await UpdateOrderTotalPrice(orderDetail.OrderID);

                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật: " + ex.Message);
                }
            }

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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var orderDetail = await _context.OrderDetails.FindAsync(id);
                    if (orderDetail == null)
                    {
                        return NotFound();
                    }

                    // Hoàn trả số lượng sản phẩm (isReturn = true)
                    await UpdateProductQuantity(orderDetail.ProductType, orderDetail.ProductID, orderDetail.Quantity, true);

                    // Xóa OrderDetail
                    _context.OrderDetails.Remove(orderDetail);
                    await _context.SaveChangesAsync();

                    // Cập nhật tổng tiền đơn hàng
                    await UpdateOrderTotalPrice(orderDetail.OrderID);

                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Có lỗi xảy ra khi xóa: " + ex.Message);
                    return View("Delete", await _context.OrderDetails
                        .Include(o => o.Order)
                        .FirstOrDefaultAsync(m => m.OrderDetailID == id));
                }
            }
        }

        private async Task ReturnProductQuantity(ProductType productType, int productId, int quantity)
        {
            switch (productType)
            {
                case ProductType.Laptop:
                    var laptop = await _context.Laptops.FindAsync(productId);
                    if (laptop != null)
                    {
                        laptop.IsSold = false;
                        _context.Update(laptop);
                    }
                    break;

                case ProductType.RAM:
                    var ram = await _context.RAMs.FindAsync(productId);
                    if (ram != null)
                    {
                        ram.Quantity += quantity;
                        _context.Update(ram);
                    }
                    break;

                case ProductType.LaptopCharger:
                    var charger = await _context.LaptopChargers.FindAsync(productId);
                    if (charger != null)
                    {
                        charger.Quantity += quantity;
                        _context.Update(charger);
                    }
                    break;

                case ProductType.LaptopScreen:
                    var screen = await _context.LaptopScreens.FindAsync(productId);
                    if (screen != null)
                    {
                        screen.Quantity += quantity;
                        _context.Update(screen);
                    }
                    break;

                case ProductType.LaptopBattery:
                    var battery = await _context.LaptopBatteries.FindAsync(productId);
                    if (battery != null)
                    {
                        battery.Quantity += quantity;
                        _context.Update(battery);
                    }
                    break;

                case ProductType.StorageDevice:
                    var storage = await _context.StorageDevices.FindAsync(productId);
                    if (storage != null)
                    {
                        storage.Quantity += quantity;
                        _context.Update(storage);
                    }
                    break;

                case ProductType.Service:
                    // Dịch vụ không cần cập nhật số lượng
                    break;

                default:
                    throw new ArgumentException("Loại sản phẩm không hợp lệ");
            }

            await _context.SaveChangesAsync();
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailID == id);
        }
    }
}
