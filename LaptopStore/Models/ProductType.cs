using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public enum ProductType
    {
        [Display(Name = "Chọn loại sản phẩm")]
        KhongCo,

        [Display(Name = "Laptop")]
        Laptop,

        [Display(Name = "Sạc")]
        LaptopCharger,

        [Display(Name = "RAM")]
        RAM,

        [Display(Name = "Màn hình")]
        LaptopScreen,

        [Display(Name = "Pin")]
        LaptopBattery,

        [Display(Name = "Ổ cứng")]
        StorageDevice,

        [Display(Name = "Dịch vụ")]
        Service
    }

}
