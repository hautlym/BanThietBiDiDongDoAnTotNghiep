using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.EF;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BanThietBiDiDongDATN.Application.Catalog.System
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRoles> _roleManager;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;
        private readonly BanThietBiDiDongDATNDbContext _context;
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRoles> roleManager, IConfiguration config, IStorageService storageService, BanThietBiDiDongDATNDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _storageService = storageService;
            _context = context;
        }
        public async Task<ApiResult<bool>> AdminChangePassword(Guid id, AdminUpdatePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            }
            user.UpdateAt = DateTime.Now;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }

            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }
        public async Task<ApiResult<string>> Authencate(LoginRequest request)
        {
            var user = await _userManager.Users.Where(x => x.UserName.Equals(request.Username) || x.Email.Equals(request.Username)).FirstOrDefaultAsync();
            if (user == null)
                return new ApiErrorResult<string>("Sai tài khoản hoặc mật khẩu");
            if (!user.EmailConfirmed)
            {
                return new ApiErrorResult<string>("Tài khoản không tồn tại");
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.remember, true);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<string>("Sai tài khoản hoặc mật khẩu");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claim = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Role,string.Join(";",roles)),
                new Claim(ClaimTypes.Name, user.LastName),
                new Claim(ClaimTypes.Surname, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.UserData,user.Avatar.ToString())

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claim,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<ApiResult<bool>> delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            if (user.Avatar != null)
            {
                _storageService.DeleteFileAsync("user", user.Avatar);
            }
            var reult = await _userManager.DeleteAsync(user);
            if (reult.Succeeded)
                return new ApiSuccessResult<bool>();

            return new ApiErrorResult<bool>("Xóa không thành công");
        }

        public async Task<ApiResult<UserViewModels>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserViewModels>("User không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = new UserViewModels()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                Dob = user.Dob,
                Id = user.Id,
                LastName = user.LastName,
                Address = user.Address,
                Roles = roles,
                Gender = user.Gender.ToString(),
                Avatar = user.Avatar,
                LastSignIn = user.LastSignIn,
                CreateAt = user.CreateAt,
                UpdateAt = user.UpdateAt,
            };
            return new ApiSuccessResult<UserViewModels>(userVm);
        }

        public async Task<ApiResult<PageResult<UserViewModels>>> GetUserPaging(GetUserPagingRequest request)
        {
            try
            {
                var query = _userManager.Users;
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.UserName.Contains(request.Keyword)
                     || x.PhoneNumber.Contains(request.Keyword) || x.LastName.Contains(request.Keyword) || x.FirstName.Contains(request.Keyword) ||
                     x.Email.Contains(request.Keyword));
                }

                var totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(
                      x => new UserViewModels()
                      {
                          FirstName = x.FirstName,
                          LastName = x.LastName,
                          Email = x.Email,
                          Id = x.Id,
                          PhoneNumber = x.PhoneNumber,
                          UserName = x.UserName,
                          Dob = x.Dob,
                          Address = x.Address,
                          Avatar = x.Avatar,
                          EmailVerify = x.EmailConfirmed,
                          LastSignIn = x.LastSignIn,
                          CreateAt = x.CreateAt,
                          UpdateAt = x.UpdateAt,
                      }).ToListAsync();
                foreach (var item in data)
                {
                    var role = await _userManager.GetRolesAsync(new AppUser() { Id = item.Id });
                    item.Roles = role.ToList();
                }
               
                var PageResult = new PageResult<UserViewModels>()
                {
                    TotalRecords = totalRow,
                    Items = data,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                };
                return new ApiSuccessResult<PageResult<UserViewModels>>(PageResult);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<PageResult<UserViewModels>>("lỗi");
            }
        }
        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            try
            {
                var userExist = await _userManager.FindByNameAsync(request.Username);
                if (userExist != null)
                {
                    return new ApiErrorResult<bool>("Tài khoản đã tồn tại");
                }

                if (await _userManager.FindByEmailAsync(request.Email) != null)
                {
                    return new ApiErrorResult<bool>("Email đã tồn tại");
                }
                var user = new AppUser()
                {
                    Dob = request.Dob,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.Username,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Gender = Convert.ToBoolean(request.Gender),
                    CreateAt = DateTime.Now
                };
                if (request.Avatar != null)
                {
                    user.Avatar = await this.SaveFile(request.Avatar);
                }
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var role = await _roleManager.Roles.Where(x => x.Name.Equals("Customer")).FirstOrDefaultAsync();
                    await RoleAssign(user.Id, new RolesAssignRequest() { Id = role.Id, roles = role });
                    return new ApiSuccessResult<bool>();
                }
                return new ApiErrorResult<bool>("Đăng ký không thành công");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>("Đăng ký không thành công");
            }

        }
        public async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), "user", fileName);
            return fileName;
        }
        public async Task<ApiResult<bool>> RoleAssign(Guid id, RolesAssignRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return new ApiErrorResult<bool>("Tài khoản không tồn tại");
                }

                var listRole = _roleManager.Roles.ToList();
                var userRoles = _context.UserRoles.Where(ur => ur.UserId == user.Id);
                _context.UserRoles.RemoveRange(userRoles);
                _context.SaveChanges();
                //if (kq1)
                {
                    var kq = await _userManager.AddToRoleAsync(user, request.roles.Name);
                    if (kq.Succeeded)
                    {
                        return new ApiSuccessResult<bool>();
                    }
                }
                return new ApiErrorResult<bool>("Gán quyền không thành công");

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>("Gán quyền không thành công");
            }
        }

        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            }
            user.Dob = request.Dob;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.Gender = Convert.ToBoolean(request.Gender);
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;
            user.UpdateAt = DateTime.Now;
            if (request.Avatar != null)
            {
                if (user.Avatar != null)
                {
                    await _storageService.DeleteFileAsync("user", user.Avatar);
                }
                user.Avatar = await this.SaveFile(request.Avatar);
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("cập nhật không thành công");
        }
        public async Task<ApiResult<bool>> CheckSignedTime(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            }
            user.LastSignIn = DateTime.Now;
            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return new ApiSuccessResult<bool>();
                }
            }
            catch (Exception ex) { }

            return new ApiErrorResult<bool>("cập nhật không thành công");
        }
        public async Task<ApiResult<bool>> ChangePassword(Guid id, UpdatePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            }
            user.UpdateAt = DateTime.Now;
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Mật khẩu không chính xác");
            }

        }
    }
}
