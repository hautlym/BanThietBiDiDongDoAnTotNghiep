using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BanThietBiDiDongDATN.Application.Catalog.Vouchers.Dtos
{
    public class CreateVoucherRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập tên voucher")]
        public string VoucherName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mã code")]
        public string VoucherCode { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giảm giá")]
        public double Discount { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày bắt đầu")]
        public DateTime BeginDate { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày kết thúc")]
        public DateTime ExpiredDate { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public int Quantity { get; set; }
    }
}
