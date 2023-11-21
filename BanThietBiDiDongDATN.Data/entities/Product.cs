using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BanThietBiDiDongDATN.Data.Interfaces;
using BanThietBiDiDongDATN.Data.Enums;
using System.Runtime.InteropServices;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class Product : IDateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string ProductName { get; set; }
        [Required]
        public double ProductPrice { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public StatusProduct status { get; set; } = StatusProduct.Stock;
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(max)")]
        public string ProductDescription { get; set; }
        public int BrandId { get; set; }
        public double Discount { get; set; }
        public bool isActived { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public Brand brand { get; set; }
        public Category category { get; set; }
        public List<ProductImage> productImgs { get; set; }
        public List<RatingProduct> ratingProducts { get; set; }
        public List<OrderDetail> orderDetails { get; set; }
    }
}
