using System.Reflection;
using System.Runtime.Serialization;

namespace BanThietBiDiDongDATN.Data.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Đang xử lí")]
        Pending,
        [EnumMember(Value = "Đang giao hàng")]
        Shipping,
        [EnumMember(Value = "Giao hàng thành công")]
        Shipped,
        [EnumMember(Value = "Thành công")]
        Complete,
        [EnumMember(Value = "Hủy đơn")]
        Cancel

    }
    public static class EnumExtensions
    {
        public static string GetEnumMemberValue(Enum value)
        {
            Type type = value.GetType();
            MemberInfo[] memInfo = type.GetMember(value.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return ((EnumMemberAttribute)attrs[0]).Value;
            }

            return value.ToString();
        }
    }
}
