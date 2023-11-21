using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BanThietBiDiDongDATN.Data.Interfaces;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class Voucher : IDateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string VoucherName { get; set; }
        [MaxLength(200)]
        [Required]
        [Column(TypeName = "varchar(200)")]
        public string VoucherCode { get; set; }
        public double Discount { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        [Required]
        public int Quantity { get; set; }

    }
}
