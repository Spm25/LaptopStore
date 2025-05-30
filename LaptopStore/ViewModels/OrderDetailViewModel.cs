using LaptopStore.Models; // Để dùng ProductType
using System.ComponentModel.DataAnnotations; // Để dùng DisplayAttribute
using System.Linq; // Để dùng First() và Cast<>()

namespace LaptopStore.ViewModels
{
    public class OrderDetailViewModel
    {
        public int OrderDetailID { get; set; }
        public int ProductID { get; set; }
        public ProductType ProductType { get; set; }

        [Display(Name = "Sản phẩm/Dịch vụ")]
        public string ProductName { get; set; } = "N/A";

        [Display(Name = "Mô tả/Đặc tính")]
        public string ProductDescription { get; set; } = "N/A";

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Đơn giá")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ")]
        public float UnitPrice { get; set; }

        [Display(Name = "Thành tiền")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ")]
        public float TotalLinePrice => Quantity * UnitPrice;

        [Display(Name = "Bảo hành")]
        public string WarrantyPeriod { get; set; } = "N/A";

        [Display(Name = "Loại")]
        public string ProductTypeName
        {
            get
            {
                var memberInfo = ProductType.GetType().GetMember(ProductType.ToString()).FirstOrDefault();
                if (memberInfo != null)
                {
                    var displayAttribute = memberInfo.GetCustomAttributes(typeof(DisplayAttribute), false)
                                                     .Cast<DisplayAttribute>()
                                                     .FirstOrDefault();
                    if (displayAttribute != null)
                    {
                        return displayAttribute.Name ?? ProductType.ToString();
                    }
                }
                return ProductType.ToString();
            }
        }
    }
}