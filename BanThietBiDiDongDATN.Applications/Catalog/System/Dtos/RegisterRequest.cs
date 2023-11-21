using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.System.Dtos
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Vui lòng điền họ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền tên")]
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        [Required(ErrorMessage = "Vui lòng điền địa chỉ")]
        public string Address { get; set; }
        public IFormFile? Avatar { get; set; }
        public string Gender { get; set; }
        [Required(ErrorMessage = "Vui lòng điền email")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng điền số điện thoại")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng điền tên người dùng")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng điền mật khẩu")]
        [MinLength(6, ErrorMessage= "Mật khẩu tối thiểu có 6 kí tự")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage= "Mật khẩu có ít nhất 1 chữ hoa, 1 chữ thường,1 kí chữ số và 1 kí tự đặc biệt ")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Mật khẩu không giống nhau")]
        [Required(ErrorMessage = "Vui lòng điền xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }
    }
}
