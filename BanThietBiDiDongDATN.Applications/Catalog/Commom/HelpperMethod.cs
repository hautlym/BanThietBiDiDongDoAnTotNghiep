using BanThietBiDiDongDATN.Application.Catalog.Carts;
using BanThietBiDiDongDATN.Data.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Commom
{
    public static class HelpperMethod
    {
        public static string FormatRemainingTime(TimeSpan remainingTime)
        {
            if (remainingTime.TotalMinutes < 1)
            {
                return "0 phút";
            }

            string formattedTime = "";

            if (remainingTime.Days > 0)
            {
                formattedTime += $"{remainingTime.Days} ngày, ";
            }
            formattedTime += $"{remainingTime.Hours} giờ, {remainingTime.Minutes} phút";

            return formattedTime;
        }
        public static string ChuyenDoiThuSangChu(DayOfWeek thu)
        {
            switch (thu)
            {
                case DayOfWeek.Sunday:
                    return "Chủ Nhật";
                case DayOfWeek.Monday:
                    return "Thứ Hai";
                case DayOfWeek.Tuesday:
                    return "Thứ Ba";
                case DayOfWeek.Wednesday:
                    return "Thứ Tư";
                case DayOfWeek.Thursday:
                    return "Thứ Năm";
                case DayOfWeek.Friday:
                    return "Thứ Sáu";
                case DayOfWeek.Saturday:
                    return "Thứ Bảy";
                default:
                    return string.Empty;
            }
        }
    }
}
