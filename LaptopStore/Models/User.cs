using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public enum UserRole
    {
        Admin = 1,      // Quản lý
        Staff = 2       // Nhân viên
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        //public string FullName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
