using BanThietBiDiDongDATN.Application.Catalog.Products.Dtos;

namespace BanThietBiDiDongDATN.MvcApp.Models
{
    public class HomeModel
    {

        public List<ProductViewModel> BannerProduct { get; set; }
        public List<ProductViewModel> NewProduct { get; set; }
        public List<ProductViewModel> NewProductPhone { get; set; }
        public List<ProductViewModel> NewProductHeadPhone{ get; set; }
        public List<ProductViewModel> NewProductCharge { get; set; }
        public List<ProductViewModel> NewOtherProduuct { get; set; }
        public List<ProductViewModel> TopSaleProduct { get; set; }
        public List<ProductViewModel> RecomandProduct { get; set; }
    }
}
