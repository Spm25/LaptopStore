using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public enum UserRole
    {
        [Display(Name = "Quản trị viên")] // Thêm dòng này
        Admin = 1,      // Quản lý
        [Display(Name = "Nhân viên")]    // Thêm dòng này
        Staff = 2       // Nhân viên
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required(ErrorMessage = "Tên người dùng không được để trống.")] // Thêm Required
        public string UserName { get; set; }
        //public string FullName { get; set; } // Giữ nguyên nếu bạn đang comment nó
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}