using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        // Cập nhật StringLength để phù hợp với độ dài số điện thoại Việt Nam (thường là 10 hoặc 11 số)
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có từ 10 đến 11 chữ số.")]
        [RegularExpression(@"^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Số điện thoại không đúng định dạng Việt Nam.")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự.")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
    }
}