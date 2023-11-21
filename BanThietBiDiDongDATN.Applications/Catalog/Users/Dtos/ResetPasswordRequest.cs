using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Users.Dtos
{
    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; }
        public  string code { get; set; }
        public string UserId { get; set; }
    }
}
