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
        public string CPU { get; set; }
        public string RAM { get; set; }
        public string StorageType { get; set; }
        public string StorageSize { get; set; }
        public string GPU { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }
}
