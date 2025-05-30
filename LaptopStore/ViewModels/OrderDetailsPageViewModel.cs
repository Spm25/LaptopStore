using LaptopStore.Models; // Để dùng Customer, User
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Để dùng DisplayAttribute

namespace LaptopStore.ViewModels
{
    public class OrderDetailsPageViewModel
    {
        public int OrderID { get; set; }

        [Display(Name = "Ngày đặt hàng")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Trạng thái thanh toán")]
        public bool Paid { get; set; }

        [Display(Name = "Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ")]
        public float OrderTotalPrice { get; set; }

        public CustomerViewModel CustomerInfo { get; set; }
        public UserViewModel UserInfo { get; set; }

        public List<OrderDetailViewModel> OrderDetails { get; set; }

        public OrderDetailsPageViewModel()
        {
            OrderDetails = new List<OrderDetailViewModel>();
            CustomerInfo = new CustomerViewModel();
            UserInfo = new UserViewModel();
        }
    }

    public class CustomerViewModel
    {
        public int CustomerID { get; set; }
        [Display(Name = "Tên khách hàng")]
        public string FullName { get; set; } = "N/A";
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; } = "N/A";
        public string Email { get; set; } = "N/A";
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; } = "N/A";
    }

    public class UserViewModel
    {
        public int UserID { get; set; }
        [Display(Name = "Tên nhân viên")]
        public string UserName { get; set; } = "N/A";
        // Bạn có thể thêm các thuộc tính khác của User nếu cần hiển thị
    }
}