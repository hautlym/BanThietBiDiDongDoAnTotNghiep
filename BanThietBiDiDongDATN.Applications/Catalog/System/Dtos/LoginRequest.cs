using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.System.Dtos
{
    public class LoginRequest
    {
        [Required(ErrorMessage= "Vui lòng nhập tài khoản")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string Password { get; set; }
        public bool remember { get; set; }
    }
}
