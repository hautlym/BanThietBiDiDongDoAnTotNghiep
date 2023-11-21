using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Vouchers.Dtos
{
    public class VoucherViewModel
    {
        public int Id { get; set; }
        public string VoucherName { get; set; }
        public string VoucherCode { get; set; }
        public double Discount { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int Quantity { get; set; }
    }
}
