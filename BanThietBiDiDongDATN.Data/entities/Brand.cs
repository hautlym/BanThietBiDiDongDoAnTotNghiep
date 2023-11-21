using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BanThietBiDiDongDATN.Data.Interfaces;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class Brand : IDateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string BrandName { get; set; }
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string? logo { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string description { get; set; }
        public List<Product> products { get; set; }
    }
}
