using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSPermissions.Application.Services.Permissions.Interfaces;
using NSPermissions.Domain.Dtos;
using Serilog;

namespace NSPermissions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController(IPermissionService _service) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            Log.Information("Operation: get");
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermission(int id)
        {
            Log.Information("Operation: get");
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> RequestPermission([FromBody] PermissionDto dto)
        {
            Log.Information("Operation: create");
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetPermission), new { id = result.Id }, result);
        }

        [HttpPut("modify/{id}")]
        public async Task<IActionResult> ModifyPermission(int id, [FromBody] PermissionDto dto)
        {
            Log.Information("Operation: modify");
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            Log.Information("Operation: delete");
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
