using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [MaxLength(250)]
        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string ImagePath { get; set; }
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public DateTime DateCreated { get; set; }
        public int SortOrder { get; set; }
        public long FileSize { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
    }
}
