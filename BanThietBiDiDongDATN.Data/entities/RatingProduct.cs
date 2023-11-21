using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class RatingProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public double NumberRating { get; set; }
        [MaxLength(250)]
        [Column(TypeName = "nvarchar(250)")]
        public string Content { get; set; }
        public int productId { get; set; }
        public Product product { get; set; }
    }
}
