using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.System.Dtos
{
    public class UserViewModels
    {
        public Guid Id { get; set; }

        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime Dob { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        public IList<string> Roles { get; set; }
        public string? Avatar { get; set; }
        public bool? EmailVerify { get; set; }
        public string Gender { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set;}
        public DateTime? LastSignIn { get; set; }
    }
}
