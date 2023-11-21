using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.EF;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BanThietBiDiDongDATN.Application.Catalog.System
{

    public class RolesService : IRolesService
    {
        private readonly RoleManager<AppRoles> _roleManager;
        private readonly BanThietBiDiDongDATNDbContext _context;
        public RolesService(RoleManager<AppRoles> roleManager, BanThietBiDiDongDATNDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateRole(CreateRoleRequest request)
        {
            var role = new AppRoles()
            {
                Id = Guid.NewGuid(),
                Name = request.RoleName,
                NormalizedName = request.RoleName.ToUpper(),
                Description = request.Description,
            };
            var checkExist =await _roleManager.FindByNameAsync(role.Name);
            if(checkExist != null)
            {
                return new ApiErrorResult<bool>("Quyền này đã tồn tại");
            }
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
               return new ApiErrorResult<bool>("Thêm quyền không thành công");
            }
        }

        public async Task<ApiResult<bool>> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return new ApiErrorResult<bool>("Quyền không tồn tại");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Xóa quyền không thành công");
            }
        }

        public async Task<List<RolesViewModel>> GetAll()
        {
            var roles = await _roleManager.Roles
                   .Select(x => new RolesViewModel()
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Description = x.Description
                   }).ToListAsync();

            return roles;
        }

        public async Task<ApiResult<RolesViewModel>> GetRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var roleVM = new RolesViewModel() { Id = role.Id, Name = role.Name, Description = role.Description };
            return new ApiSuccessResult<RolesViewModel>(roleVM);
        }
        public async Task<ApiResult<PageResult<RolesViewModel>>> GetRolesPaging(GetRolePagingRequest request)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword));
            }
            var totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(
                x => new RolesViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                }).ToListAsync();
            var PageResult = new PageResult<RolesViewModel>()
            {
                TotalRecords = totalRow,
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return new ApiSuccessResult<PageResult<RolesViewModel>>(PageResult);
        }
        public async Task<ApiResult<bool>> UpdateRole(UpdateRoleRequest request)
        {
            var role = await _roleManager.FindByIdAsync(request.id);
            if (role == null)
            {
                return new ApiErrorResult<bool>("Quyền không tồn tại");

            }
            var checkExist = await _roleManager.FindByNameAsync(request.RoleName);
            if (checkExist != null)
            {
                return new ApiErrorResult<bool>("Quyền đã tồn tại");

            }
            role.Name = request.RoleName;
            role.NormalizedName = request.RoleName.ToUpper();
            role.Description = request.Description;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            else
            {
                return new ApiErrorResult<bool>("Sửa quyền không thành công");
            }
            
        }
        public async Task<ApiResult<int>> getTotalUserWithRole(string id)
        {
            var role = _roleManager.Roles.Where(x=>x.Id.ToString() == id);
            if (role != null)
            {
                var usersWithRole =await _context.UserRoles
                    .Where(ur => ur.RoleId.ToString() == id)
                    .CountAsync();
                return new ApiSuccessResult<int>(usersWithRole); ;
            }
            else
            {
                return new ApiErrorResult<int>("lỗi");
            }
        }
    }
}
