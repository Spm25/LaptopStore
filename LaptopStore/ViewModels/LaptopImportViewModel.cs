using System.Collections.Generic;
using LaptopStore.Models; 
namespace LaptopStore.ViewModels
{
    
    public class LaptopImportEntry
    {
        public Laptop LaptopData { get; set; }
        public int OriginalRowNumber { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsValidForImport => !Errors.Any(); // Hợp lệ nếu không có lỗi
        public bool HasWarnings => Warnings.Any();
        public string RowSummary // Thuộc tính để hiển thị tóm tắt dòng trên view
        {
            get
            {
                if (LaptopData != null && !string.IsNullOrWhiteSpace(LaptopData.Model))
                {
                    return $"Dòng {OriginalRowNumber}: {LaptopData.Brand} {LaptopData.Model} (S/N: {LaptopData.SerialNumber ?? "N/A"})";
                }
                return $"Dòng {OriginalRowNumber}: Dữ liệu không đủ để xác định";
            }
        }
    }

    public class ImportConfirmationViewModel
    {
        public List<LaptopImportEntry> EntriesToReview { get; set; } = new List<LaptopImportEntry>();
        public int ValidCount => EntriesToReview.Count(e => e.IsValidForImport && !e.HasWarnings);
        public int WarningCount => EntriesToReview.Count(e => e.IsValidForImport && e.HasWarnings);
        public int ErrorCount => EntriesToReview.Count(e => !e.IsValidForImport);
    }
}
