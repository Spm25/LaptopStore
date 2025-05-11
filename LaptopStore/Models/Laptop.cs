using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public class Laptop
    {
        [Key]
        public int LaptopID { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        public string SerialNumber { get; set; }
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string Storage { get; set; }
        public string GPU { get; set; }

        public float ImportPrice { get; set; }
        public float SellingPrice { get; set; }

        public string Description { get; set; }
        public string ImageURL { get; set; }

        public float ScreenSize { get; set; }          // inch
        public string OperatingSystem { get; set; }    // Hệ điều hành
        public int BatteryHealth { get; set; }         // % tình trạng pin
    }

}
