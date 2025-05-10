using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public class LaptopScreen
    {
        [Key]
        public int ScreenID { get; set; }
        [Required]
        public string Resolution { get; set; }
        [Required]
        public string ScreenType { get; set; }
        public string UsedStatus { get; set; }
        public int Repaired { get; set; }
        public int Quantity { get; set; }
        public string Quality { get; set; }
    }
}
