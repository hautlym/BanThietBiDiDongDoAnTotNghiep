using BanThietBiDiDongDATN.Application.Catalog.Products.Dtos;

namespace BanThietBiDiDongDATN.MvcApp.Models
{
    public class DeltailModel
    {
        public ProductViewModel Product { get; set; }
        public List<ProductViewModel> RelatedProduct { get; set; }
    }
}
