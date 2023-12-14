using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using BanThietBiDiDongDATN.Data.Enums;
using BanThietBiDiDongDATN.Data.Interfaces;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class Order:IDateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string ShipName { get; set; }
        [MaxLength(200)]
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string ShipAddress { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string ShipEmail { get; set; }
        [MaxLength(200)]
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string ShipNumberPhone { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string? ShipDescription { get; set; }
        public OrderStatus status { get; set; }
        public string typeOrder {get;set;}
        public int? voucherId { get; set; }
        public TypeCheckOut typePayment { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid? AppUserId { get; set; }
        public AppUser user { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
