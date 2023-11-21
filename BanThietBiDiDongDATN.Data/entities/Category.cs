using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BanThietBiDiDongDATN.Data.Interfaces;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class Category : IDateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string CategoryName { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; }
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public List<Product> Products { get; set; }
    }
}
