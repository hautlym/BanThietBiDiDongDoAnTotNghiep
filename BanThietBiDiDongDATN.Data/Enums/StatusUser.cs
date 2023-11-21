using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Data.Enums
{
    public enum StatusUser
    {
        [EnumMember(Value = "Hoạt động")]
        Actived,
        [EnumMember(Value = "Không hoạt động")]
        NonActived,
        [EnumMember(Value = "Khóa")]
        Lock
    }
}
