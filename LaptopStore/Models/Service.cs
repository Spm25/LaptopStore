using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        public float Price { get; set; }

        public string Description { get; set; }
    }

}
