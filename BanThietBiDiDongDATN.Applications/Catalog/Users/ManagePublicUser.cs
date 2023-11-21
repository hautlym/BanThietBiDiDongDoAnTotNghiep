using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using System;

namespace BanThietBiDiDongDATN.Application.Catalog.Users
{
    public class ManagePublicUser : IManagePublicUser
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public ManagePublicUser(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterRequest> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public Task<ApiResult<string>> LoginPublic(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> CheckSignedTime(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> deletePublic(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<UserViewModels>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> RegisterPublic(RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdatePublic(Guid id, UserUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
