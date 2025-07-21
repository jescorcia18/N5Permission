

using NSPermissions.Domain.Dtos;

namespace NSPermissions.Application.Services.Permissions.Interfaces;

public interface IPermissionService
{
    Task<IEnumerable<PermissionDto>> GetAllAsync();
    Task<PermissionDto?> GetByIdAsync(int id);
    Task<PermissionDto> CreateAsync(PermissionDto dto);
    Task<PermissionDto?> UpdateAsync(int id, PermissionDto dto);
    Task<bool> DeleteAsync(int id);
}
