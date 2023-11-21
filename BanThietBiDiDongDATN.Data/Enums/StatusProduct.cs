using System.Runtime.Serialization;

namespace BanThietBiDiDongDATN.Data.Enums
{
    public enum StatusProduct
    {
        [EnumMember(Value = "In Stock")]
        Stock,
        [EnumMember(Value = "Out of Stock")]
        SoldOut
    }
}
