
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using BanThietBiDiDongDATN.Application.Catalog.System;
using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BTL_KTPM.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _roleManager;

        public RolesController(IRolesService roleService)
        {
            _roleManager = roleService;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetRolePagingRequest request)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            var products = await _roleManager.GetRolesPaging(request);
            return Ok(products);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleManager.GetAll();
            return Ok(roles);
        }
        [HttpPost]
        public async Task<IActionResult> PostRole([FromBody] CreateRoleRequest request)
        {
            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState); 
            }
            var result = await _roleManager.CreateRole(request);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("getTotalUser")]
        public async Task<IActionResult> GetTotalUserWithRole(string Id)
        {
            var number = await _roleManager.getTotalUserWithRole(Id);
            return Ok(number);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {

            var role = await _roleManager.GetRoleById(id);
            if (role == null)
            {
                return NotFound(role);
            }
            return Ok(role);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, [FromBody] UpdateRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _roleManager.UpdateRole(request);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _roleManager.DeleteRole(id);
            if (result.IsSuccessed)
            {

                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
