using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LaptopStore.Models
{
    public abstract class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự.")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống.")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng tồn kho không được để trống.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không thể là số âm.")]
        [Display(Name = "Số lượng tồn")]
        public int StockQuantity { get; set; }

        [StringLength(100)]
        [Display(Name = "Thương hiệu")]
        public string Brand { get; set; }

        [Display(Name = "Đang giảm giá")]
        public bool IsSale { get; set; } = false;

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Giá giảm")]
        public decimal? SalePrice { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại sản phẩm.")]
        [Display(Name = "Loại sản phẩm")]
        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        protected Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
    }
}
