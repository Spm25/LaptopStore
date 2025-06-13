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
    }

    public class EmployeeRevenueViewModel
    {
        public string EmployeeName { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}