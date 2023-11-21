using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;

namespace BanThietBiDiDongDATN.Application.Catalog.Users
{
    public interface IManagePublicUser
    {
        Task<ApiResult<string>> LoginPublic(LoginRequest request);
        Task<ApiResult<bool>> RegisterPublic(RegisterRequest request);
        Task<ApiResult<bool>> UpdatePublic(Guid id, UserUpdateRequest request);

        Task<ApiResult<bool>> deletePublic(Guid id);
        Task<ApiResult<UserViewModels>> GetById(Guid id);
        //Task<ApiResult<bool>> ChangePassword(Guid id, UpdatePasswordRequest request);
        Task<ApiResult<bool>> CheckSignedTime(Guid id);
    }
}
