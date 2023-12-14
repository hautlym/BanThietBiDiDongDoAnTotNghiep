using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Data.Enums
{
    public enum TypeCheckOut
    {
        [EnumMember(Value = "Thanh toán tiền mặt")]
        COD,

        [EnumMember(Value = "Thanh toán trực tuyến")]
        Online,

    }
}
