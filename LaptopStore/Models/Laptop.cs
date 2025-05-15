using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string Description { get; set; } // Không required
        public string ImageURL { get; set; }    // Không required

        public float ScreenSize { get; set; }          // inch
        public string OperatingSystem { get; set; }    // Hệ điều hành
        public int BatteryHealth { get; set; }         // % tình trạng pin

        [Required]
        public bool IsSold { get; set; } = false; // Mặc định là chưa bán
    }
}
