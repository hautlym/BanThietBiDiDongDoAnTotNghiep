using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.System
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);

        Task<ApiResult<PageResult<UserViewModels>>> GetUserPaging(GetUserPagingRequest request);
        Task<ApiResult<bool>> delete(Guid id);
        Task<ApiResult<UserViewModels>> GetById(Guid id);
        public Task<ApiResult<bool>> RoleAssign(Guid id, RolesAssignRequest request);
        Task<ApiResult<bool>> ChangePassword(Guid id, UpdatePasswordRequest request);
        Task<ApiResult<bool>> AdminChangePassword(Guid id, AdminUpdatePasswordRequest request);
        Task<ApiResult<bool>> CheckSignedTime(Guid id);

    }
}
