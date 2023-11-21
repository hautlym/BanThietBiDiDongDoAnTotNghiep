using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.System.Dtos
{
    public class CreateRoleRequest
    {
        [Required(ErrorMessage = "Vui lòng điền tên quyền")]
        public string RoleName{get;set; }
        [Required(ErrorMessage = "Vui lòng điền mô tả")]
        public string Description { get;set; }
    }
}
