using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;

namespace BanThietBiDiDongDATN.ApiIntegration.Service.RolesApiClient
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RolesViewModel>>> GetAll();
        Task<ApiResult<PageResult<RolesViewModel>>> GetRolePaging(GetRolePagingRequest request);
        Task<ApiResult<bool>> CreateRole(CreateRoleRequest request);
        Task<ApiResult<bool>> UpdateRole(string id,UpdateRoleRequest request);
        Task<ApiResult<bool>> DeleteRole(string id);
        Task<ApiResult<int>> GetTotalUserWithRole(string roleName);
        Task<ApiResult<RolesViewModel>> GetRoleById(string id);
    }
}
