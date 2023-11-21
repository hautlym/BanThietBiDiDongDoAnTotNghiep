using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.System
{
    public interface IRolesService
    {
        public Task<List<RolesViewModel>> GetAll();
        Task<ApiResult<PageResult<RolesViewModel>>> GetRolesPaging(GetRolePagingRequest request);
        public Task<ApiResult<bool>> CreateRole(CreateRoleRequest request);
        public Task<ApiResult<bool>> UpdateRole(UpdateRoleRequest request);
        public Task<ApiResult<bool>> DeleteRole(string id);
        public Task<ApiResult<int>> getTotalUserWithRole(string id);
        public Task<ApiResult<RolesViewModel>> GetRoleById(string id);

    }
}
