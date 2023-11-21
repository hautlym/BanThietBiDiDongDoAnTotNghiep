using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;

namespace BanThietBiDiDongDATN.ApiIntegration.Service.UserApiClient
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<PageResult<UserViewModels>>> GetUserPaging(GetUserPagingRequest request);
        Task<ApiResult<bool>> RegisterUser(RegisterRequest request);
        Task<ApiResult<bool>> Delete(Guid id);
        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);
        Task<ApiResult<UserViewModels>> GetById(Guid id);
        Task<ApiResult<bool>> RoleAssign(Guid id, RolesAssignRequest request);
        Task<ApiResult<bool>> ChangePassword(Guid id, UpdatePasswordRequest request);
        Task<ApiResult<bool>> AdminChangePassword(Guid id, AdminUpdatePasswordRequest request);
        Task<ApiResult<bool>> CheckSignedTime(Guid id);
    }
}
