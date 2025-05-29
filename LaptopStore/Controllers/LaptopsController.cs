using Microsoft.AspNetCore.Http; // For IFormFile
using OfficeOpenXml; // For ExcelPackage
using System;
using System.Collections.Generic;
using System.IO; // For MemoryStream, Path, Directory
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // For AnyAsync, SaveChangesAsync
using LaptopStore.Data; // Your DbContext
using LaptopStore.Models; // Your Laptop model
using AspNetCoreHero.ToastNotification.Abstractions; // For INotyfService
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LaptopStore.ViewModels;
using System.Text.Json;

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


        // GET: Laptops/Import (Hiển thị form upload ban đầu)
        public IActionResult Import()
        {
            return View(); // View này là Import.cshtml đã có
        }

        // POST: Laptops/ProcessExcelForConfirmation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessExcelForConfirmation(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _notifyService.Error("Vui lòng chọn file Excel.");
                return RedirectToAction(nameof(Import));
            }

            var allEntries = new List<LaptopImportEntry>();
            var serialNumbersInFile = new List<(string Serial, int RowNum)>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream)) // License đã được set trong Program.cs
                {
                    if (package.Workbook.Worksheets.Count == 0)
                    {
                        _notifyService.Error("File Excel không có sheet nào.");
                        return RedirectToAction(nameof(Import));
                    }
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    if (rowCount <= 1)
                    {
                        _notifyService.Warning("File Excel không có dữ liệu để nhập.");
                        return RedirectToAction(nameof(Import));
                    }

                    // Bước 1: Đọc tất cả các dòng, parse dữ liệu cơ bản và thu thập SerialNumber
                    for (int row = 2; row <= rowCount; row++)
                    {
                        // Kiểm tra dòng hoàn toàn trống
                        bool isRowEmpty = true;
                        for (int col = 1; col <= 15; col++) // Giả sử có 15 cột dữ liệu
                        {
                            if (worksheet.Cells[row, col].Value != null && !string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Value.ToString()))
                            {
                                isRowEmpty = false;
                                break;
                            }
                        }
                        if (isRowEmpty) continue;

                        var entry = new LaptopImportEntry { OriginalRowNumber = row };
                        var laptop = new Laptop();
                        entry.LaptopData = laptop;

                        try
                        {
                            laptop.Brand = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                            laptop.Model = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                            laptop.SerialNumber = worksheet.Cells[row, 3].Value?.ToString()?.Trim();

                            if (!string.IsNullOrEmpty(laptop.SerialNumber))
                            {
                                serialNumbersInFile.Add((laptop.SerialNumber, row));
                            }

                            laptop.CPU = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                            laptop.RAM = worksheet.Cells[row, 5].Value?.ToString()?.Trim();
                            laptop.Storage = worksheet.Cells[row, 6].Value?.ToString()?.Trim();
                            laptop.GPU = worksheet.Cells[row, 7].Value?.ToString()?.Trim();

                            string importPriceStr = worksheet.Cells[row, 8].Value?.ToString();
                            if (float.TryParse(importPriceStr, out var importPrice)) laptop.ImportPrice = importPrice;
                            else if (!string.IsNullOrWhiteSpace(importPriceStr)) entry.Errors.Add("Định dạng Giá nhập (ImportPrice) không hợp lệ.");

                            string sellingPriceStr = worksheet.Cells[row, 9].Value?.ToString();
                            if (float.TryParse(sellingPriceStr, out var sellingPrice)) laptop.SellingPrice = sellingPrice;
                            else if (!string.IsNullOrWhiteSpace(sellingPriceStr)) entry.Errors.Add("Định dạng Giá bán (SellingPrice) không hợp lệ.");

                            laptop.Description = worksheet.Cells[row, 10].Value?.ToString()?.Trim();
                            laptop.ImageURL = worksheet.Cells[row, 11].Value?.ToString()?.Trim();

                            string screenSizeStr = worksheet.Cells[row, 12].Value?.ToString();
                            if (float.TryParse(screenSizeStr, out var screenSize)) laptop.ScreenSize = screenSize;
                            else if (!string.IsNullOrWhiteSpace(screenSizeStr)) entry.Errors.Add("Định dạng Kích thước màn hình (ScreenSize) không hợp lệ.");

                            laptop.OperatingSystem = worksheet.Cells[row, 13].Value?.ToString()?.Trim();

                            string batteryHealthStr = worksheet.Cells[row, 14].Value?.ToString();
                            if (int.TryParse(batteryHealthStr, out var batteryHealth)) laptop.BatteryHealth = batteryHealth;
                            else if (!string.IsNullOrWhiteSpace(batteryHealthStr)) entry.Errors.Add("Định dạng Tình trạng pin (BatteryHealth) không hợp lệ.");

                            string isSoldStr = worksheet.Cells[row, 15].Value?.ToString();
                            if (bool.TryParse(isSoldStr, out var isSold)) laptop.IsSold = isSold;
                            else if (!string.IsNullOrWhiteSpace(isSoldStr)) entry.Errors.Add("Trạng thái Đã bán (IsSold) phải là TRUE hoặc FALSE.");

                        }
                        catch (Exception ex)
                        {
                            entry.Errors.Add($"Lỗi hệ thống khi xử lý dòng: {ex.Message}");
                        }
                        allEntries.Add(entry);
                    }
                }
            }

            // Bước 2: Xác định SerialNumber bị trùng lặp trong file
            var duplicateSerialGroups = serialNumbersInFile
                .GroupBy(s => s.Serial)
                .Where(g => g.Count() > 1)
                .ToList();

            // Bước 3: Áp dụng lỗi cho các dòng có SerialNumber trùng trong file và thực hiện các validation khác
            foreach (var entry in allEntries)
            {
                // Kiểm tra trùng lặp SerialNumber trong file
                if (!string.IsNullOrEmpty(entry.LaptopData.SerialNumber))
                {
                    var duplicateGroup = duplicateSerialGroups.FirstOrDefault(g => g.Key == entry.LaptopData.SerialNumber);
                    if (duplicateGroup != null)
                    {
                        string duplicatedRows = string.Join(", ", duplicateGroup.Select(item => item.RowNum));
                        entry.Errors.Add($"SerialNumber '{entry.LaptopData.SerialNumber}' bị trùng lặp trong file Excel ở các dòng: {duplicatedRows}.");
                    }
                }

                // Kiểm tra các trường bắt buộc (nếu chưa có lỗi nào khác nghiêm trọng hơn)
                if (string.IsNullOrWhiteSpace(entry.LaptopData.Brand))
                    entry.Errors.Add("Brand là bắt buộc.");
                if (string.IsNullOrWhiteSpace(entry.LaptopData.Model))
                    entry.Errors.Add("Model là bắt buộc.");

                // Chỉ kiểm tra với DB và các cảnh báo khác nếu dòng không có lỗi nghiêm trọng nào từ trước
                if (entry.IsValidForImport)
                {
                    // Kiểm tra SerialNumber đã tồn tại trong CSDL (Warning)
                    if (!string.IsNullOrEmpty(entry.LaptopData.SerialNumber))
                    {
                        if (await _context.Laptops.AnyAsync(l => l.SerialNumber == entry.LaptopData.SerialNumber))
                        {
                            entry.Warnings.Add($"SerialNumber '{entry.LaptopData.SerialNumber}' đã tồn tại trong CSDL.");
                        }
                    }

                    // Các cảnh báo logic nghiệp vụ khác
                    if (entry.LaptopData.SellingPrice > 0 && entry.LaptopData.ImportPrice > 0 && entry.LaptopData.SellingPrice <= entry.LaptopData.ImportPrice)
                        entry.Warnings.Add("Giá bán không cao hơn giá nhập.");
                }
            }


            if (!allEntries.Any())
            {
                _notifyService.Information("Không tìm thấy dữ liệu nào trong file để xử lý.");
                return RedirectToAction(nameof(Import));
            }

            TempData["ImportEntries"] = JsonSerializer.Serialize(allEntries);
            return RedirectToAction(nameof(ConfirmImport));
        }

        // GET: Laptops/ConfirmImport (Giữ nguyên như trước)
        public IActionResult ConfirmImport()
        {
            if (TempData["ImportEntries"] is string serializedEntries)
            {
                var entries = JsonSerializer.Deserialize<List<LaptopImportEntry>>(serializedEntries);
                var model = new ImportConfirmationViewModel { EntriesToReview = entries };

                TempData.Keep("ImportEntries");

                if (model.ErrorCount > 0 && model.ValidCount == 0 && model.WarningCount == 0)
                {
                    _notifyService.Error("Tất cả các dòng trong file Excel đều có lỗi không thể import.");
                }
                else if (model.ValidCount == 0 && model.WarningCount == 0 && model.ErrorCount == 0)
                {
                    // Trường hợp này cũng nên kiểm tra xem có entry nào không, nếu không có entries thì không có dữ liệu
                    if (!model.EntriesToReview.Any())
                    {
                        _notifyService.Information("File Excel không chứa dữ liệu hợp lệ (ví dụ: chỉ có header hoặc các dòng trống).");
                    }
                    else
                    {
                        _notifyService.Information("Không có dữ liệu hợp lệ nào được tìm thấy để import từ file.");
                    }
                }
                return View(model);
            }
            _notifyService.Error("Không có dữ liệu để xác nhận. Vui lòng thử upload lại.");
            return RedirectToAction(nameof(Import));
        }

        // POST: Laptops/ExecuteImportConfirmed (Giữ nguyên, chỉ import entry.IsValidForImport)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExecuteImportConfirmed()
        {
            if (TempData["ImportEntries"] is string serializedEntries)
            {
                var entriesToReview = JsonSerializer.Deserialize<List<LaptopImportEntry>>(serializedEntries);
                var laptopsToImport = new List<Laptop>();
                int importedWithWarnings = 0;
                int successfullyImported = 0;

                foreach (var entry in entriesToReview)
                {
                    // Chỉ import những entry không có lỗi (IsValidForImport == true)
                    if (entry.IsValidForImport)
                    {
                        // Kiểm tra lại lần cuối với CSDL trước khi thêm, phòng trường hợp dữ liệu CSDL thay đổi
                        // hoặc nếu một serial number có cảnh báo tồn tại trong CSDL nhưng người dùng vẫn muốn import
                        bool alreadyExistsInDb = false;
                        if (!string.IsNullOrEmpty(entry.LaptopData.SerialNumber))
                        {
                            alreadyExistsInDb = await _context.Laptops.AnyAsync(l => l.SerialNumber == entry.LaptopData.SerialNumber);
                        }

                        if (alreadyExistsInDb)
                        {
                            // Nếu đã tồn tại trong DB (dù đã được cảnh báo), chúng ta sẽ không thêm lại ở bước này
                            // Trừ khi bạn muốn có logic ghi đè phức tạp hơn.
                            // Thông báo này có thể đã được đưa ra ở dạng Warning.
                            continue;
                        }

                        laptopsToImport.Add(entry.LaptopData);
                        if (entry.HasWarnings)
                        {
                            importedWithWarnings++;
                        }
                        successfullyImported++;
                    }
                }

                if (laptopsToImport.Any())
                {
                    try
                    {
                        _context.Laptops.AddRange(laptopsToImport);
                        await _context.SaveChangesAsync();
                        string successMessage = $"Đã nhập thành công {successfullyImported} laptop.";
                        if (importedWithWarnings > 0)
                        {
                            successMessage += $" Trong đó có {importedWithWarnings} laptop được nhập với cảnh báo.";
                        }
                        _notifyService.Success(successMessage);
                    }
                    // ... (phần catch exceptions giữ nguyên) ...
                    catch (DbUpdateException dbEx)
                    {
                        _notifyService.Error($"Lỗi khi lưu vào database: {dbEx.InnerException?.Message ?? dbEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        _notifyService.Error($"Lỗi không xác định khi lưu: {ex.Message}");
                    }
                }
                else
                {
                    // Nếu không có lỗi trước đó từ TempData, thì đây là trường hợp không có gì hợp lệ để import
                    if (entriesToReview.All(e => !e.IsValidForImport) && entriesToReview.Any(e => !e.IsValidForImport))
                    {
                        // Không cần thông báo gì thêm vì trang ConfirmImport đã hiển thị lỗi
                    }
                    else
                    {
                        _notifyService.Information("Không có laptop nào hợp lệ để import sau khi xác nhận.");
                    }
                }
            }
            else
            {
                _notifyService.Error("Không tìm thấy dữ liệu để import. Phiên làm việc có thể đã hết hạn.");
            }
            return RedirectToAction(nameof(Index));
        }



        // Action để tải file Excel mẫu (giữ nguyên như trước)
        public IActionResult DownloadSampleExcel()
        {
            string fileName = "Laptops_Sample_New.xlsx";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sample_files", fileName);

            var sampleFileDir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(sampleFileDir))
            {
                Directory.CreateDirectory(sampleFileDir);
            }
            // OfficeOpenXml.ExcelPackage.License = OfficeOpenXml.LicenseContext.NonCommercial; // ĐÃ SET TRONG PROGRAM.CS

            if (!System.IO.File.Exists(filePath))
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Laptops");
                    // ... (Ghi header và dữ liệu mẫu như code trước đó) ...
                    worksheet.Cells[1, 1].Value = "Brand";
                    worksheet.Cells[1, 2].Value = "Model";
                    worksheet.Cells[1, 3].Value = "SerialNumber";
                    worksheet.Cells[1, 4].Value = "CPU";
                    worksheet.Cells[1, 5].Value = "RAM";
                    worksheet.Cells[1, 6].Value = "Storage";
                    worksheet.Cells[1, 7].Value = "GPU";
                    worksheet.Cells[1, 8].Value = "ImportPrice (giá nhập)";
                    worksheet.Cells[1, 9].Value = "SellingPrice (giá bán)";
                    worksheet.Cells[1, 10].Value = "Description";
                    worksheet.Cells[1, 11].Value = "ImageURL";
                    worksheet.Cells[1, 12].Value = "ScreenSize (inch)";
                    worksheet.Cells[1, 13].Value = "OperatingSystem";
                    worksheet.Cells[1, 14].Value = "BatteryHealth (%)";
                    worksheet.Cells[1, 15].Value = "IsSold (TRUE/FALSE)";

                    worksheet.Cells[2, 1].Value = "Dell"; /* ... thêm các giá trị mẫu ... */
                    worksheet.Cells[2, 15].Value = false;

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    package.SaveAs(new FileInfo(filePath));
                }
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
