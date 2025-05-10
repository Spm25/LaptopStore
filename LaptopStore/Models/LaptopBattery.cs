using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public class LaptopBattery
    {
        [Key]
        public int BatteryID { get; set; }
        [Required]
        public string LaptopModel { get; set; }
        public string Capacity { get; set; }
        public int UsedStatus { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string Quality { get; set; }
    }
}
