using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanThietBiDiDongDATN.Data.Interfaces;
using System.Text.Json.Serialization;

namespace BanThietBiDiDongDATN.Data.entities
{
    public class ProductOption:IDateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public int? ProductId { get; set; }
        public string ColorOption { get; set; }
        public  string SizeOption { get; set; }
        public string OptionPrice { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public Product product { get; set; }
        public List<WareHouse> wareHouses { get; set; }
    }
}
