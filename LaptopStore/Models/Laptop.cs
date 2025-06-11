using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStore.Models
{
    public class Laptop
    {
        [Key]
        public int LaptopID { get; set; }

        [Required(ErrorMessage = "Hãng sản xuất là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Hãng sản xuất không được vượt quá 50 ký tự.")]
        [Display(Name = "Hãng sản xuất")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Model không được vượt quá 100 ký tự.")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [StringLength(50, ErrorMessage = "Số serial không được vượt quá 50 ký tự.")]
        [Display(Name = "Số Serial")]
        public string SerialNumber { get; set; }

        [StringLength(100, ErrorMessage = "CPU không được vượt quá 100 ký tự.")]
        [Display(Name = "CPU")]
        public string CPU { get; set; }

        [StringLength(50, ErrorMessage = "RAM không được vượt quá 50 ký tự.")] // Ví dụ: "16GB DDR4"
        [Display(Name = "RAM")]
        public string RAM { get; set; }

        [StringLength(100, ErrorMessage = "Thông tin ổ cứng không được vượt quá 100 ký tự.")] // Ví dụ: "512GB SSD NVMe"
        [Display(Name = "Ổ cứng")]
        public string Storage { get; set; }

        [StringLength(100, ErrorMessage = "GPU không được vượt quá 100 ký tự.")]
        [Display(Name = "GPU (Card đồ họa)")]
        public string GPU { get; set; }

        [Required(ErrorMessage = "Giá nhập là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá nhập phải lớn hơn 0.")]
        [Display(Name = "Giá nhập")]
        [DataType(DataType.Currency)]
        public float ImportPrice { get; set; }

        [Required(ErrorMessage = "Giá bán là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0.")]
        [Display(Name = "Giá bán")]
        [DataType(DataType.Currency)]
        // Custom validation for SellingPrice >= ImportPrice can be done via IValidatableObject or in service layer
        public float SellingPrice { get; set; }

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự.")]
        [Display(Name = "Mô tả")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [StringLength(2048, ErrorMessage = "Đường dẫn hình ảnh không được vượt quá 2048 ký tự.")]
        [Url(ErrorMessage = "Đường dẫn hình ảnh không hợp lệ.")]
        [Display(Name = "Đường dẫn hình ảnh")]
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "Kích thước màn hình là bắt buộc.")]
        [Range(7, 30, ErrorMessage = "Kích thước màn hình phải trong khoảng 7 đến 30 inch.")] // Giả sử kích thước hợp lệ
        [Display(Name = "Kích thước màn hình (inch)")]
        public float ScreenSize { get; set; }

        [StringLength(50, ErrorMessage = "Hệ điều hành không được vượt quá 50 ký tự.")]
        [Display(Name = "Hệ điều hành")]
        public string OperatingSystem { get; set; }

        [Required(ErrorMessage = "Tình trạng pin là bắt buộc.")]
        [Range(0, 100, ErrorMessage = "Tình trạng pin phải từ 0 đến 100%.")]
        [Display(Name = "Tình trạng pin (%)")]
        public int BatteryHealth { get; set; }

        [Required]
        [Display(Name = "Đã bán")]
        public bool IsSold { get; set; } = false; // Mặc định là chưa bán
    }
}