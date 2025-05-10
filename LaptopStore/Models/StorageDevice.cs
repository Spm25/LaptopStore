using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public class StorageDevice
    {
        [Key]
        public int StorageID { get; set; }
        public string Type { get; set; }
        public string Capacity { get; set; }
        public int Quantity { get; set; }
        public string Quality { get; set; }
    }
}
