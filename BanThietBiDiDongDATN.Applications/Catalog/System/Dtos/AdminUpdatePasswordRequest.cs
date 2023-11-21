using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.System.Dtos
{
    public class AdminUpdatePasswordRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Vui lòng điền mật khẩu mới")]
        [MinLength(8, ErrorMessage = "Mật khẩu tối thiểu có 6 kí tự")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Mật khẩu có ít nhất 1 chữ hoa, 1 chữ thường,1 kí chữ số và 1 kí tự đặc biệt ")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng điền lại mật khẩu ")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu nhập lại không chính xác")]
        public string ConfirmPassword { get; set; }
    }
}
