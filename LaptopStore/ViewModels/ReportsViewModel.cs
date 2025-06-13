using System;
using System.Collections.Generic;

namespace LaptopStore.ViewModels
{
    public class ReportsViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int UnpaidOrdersCount { get; set; }
        public List<EmployeeRevenueViewModel> EmployeeRevenues { get; set; }

        // Thêm các thuộc tính mới cho báo cáo
        public List<ProductWarrantyViewModel> ProductsUnderWarranty { get; set; }
        public List<CustomerDebtViewModel> CustomersWithDebt { get; set; }
    }

    public class EmployeeRevenueViewModel
    {
        public string EmployeeName { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    // ViewModel mới cho sản phẩm còn bảo hành
    public class ProductWarrantyViewModel
    {
        public int OrderDetailID { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public int WarrantyPeriodMonths { get; set; } // Thời gian bảo hành theo tháng
        public int RemainingWarrantyDays { get; set; } // Số ngày bảo hành còn lại
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
    }

    // ViewModel mới cho khách hàng còn nợ
    public class CustomerDebtViewModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public decimal TotalDebt { get; set; }
        public int NumberOfUnpaidOrders { get; set; }
    }
}