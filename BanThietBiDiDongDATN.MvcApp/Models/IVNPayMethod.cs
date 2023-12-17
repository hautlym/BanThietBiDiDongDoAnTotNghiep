namespace BanThietBiDiDongDATN.MvcApp.Models
{
    public interface IVNPayMethod
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
