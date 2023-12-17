using BanThietBiDiDongDATN.Application.Catalog.Orders.Dtos;

namespace BanThietBiDiDongDATN.MvcApp.Models;

public class PaymentInformationModel
{
    public string OrderType { get; set; }
    public double Amount { get; set; }
    public string OrderDescription { get; set; }
    public string Name { get; set; }
    public CreateOrderRequest? createOrderRequest { get; set; }  
}