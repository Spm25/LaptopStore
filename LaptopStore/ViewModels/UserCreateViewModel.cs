using System.ComponentModel.DataAnnotations;
using LaptopStore.Models; // Đảm bảo include namespace của UserRole

namespace LaptopStore.ViewModels
{
    public class UserCreateViewModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Tên người dùng không được để trống.")]
        [Display(Name = "Tên người dùng")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải dài ít nhất 6 ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        [Display(Name = "Vai trò")]
        public UserRole Role { get; set; }
    }
}