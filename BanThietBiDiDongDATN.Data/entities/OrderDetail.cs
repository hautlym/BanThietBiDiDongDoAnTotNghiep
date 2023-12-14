using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class OrderDetail
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public int OptionId { get; set; }
        public double Price { get; set; }
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public Product Products { get; set; }
    }
}
