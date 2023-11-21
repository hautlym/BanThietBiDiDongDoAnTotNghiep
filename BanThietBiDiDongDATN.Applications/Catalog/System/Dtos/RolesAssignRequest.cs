using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Data.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.System.Dtos
{
    public class RolesAssignRequest
    {

        public Guid Id { get; set; }
        public AppRoles roles { get; set; }
    }
}
