using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public class RAM
    {
        [Key]
        public int RAMID { get; set; }
        public int Capacity { get; set; }
        public int Speed { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string Quality { get; set; }
    }
}
