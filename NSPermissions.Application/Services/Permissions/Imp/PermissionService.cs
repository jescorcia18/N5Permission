using Microsoft.EntityFrameworkCore;
using NSPermissions.Application.Services.Permissions.Interfaces;
using NSPermissions.Domain.Dtos;
using NSPermissions.Domain.Entities;
using NSPermissions.Domain.Interfaces;
using NSPermissions.Domain.Interfaces.UnitOfWork;
using NSPermissions.Infrastructure.Persistence;


namespace NSPermissions.Application.Services.Permissions.Imp;

public class PermissionService(IUnitOfWork _unitOfWork, IPermissionIndexService _indexService, IPermissionKafkaProducer _kafka) : IPermissionService
{
    public async Task<IEnumerable<PermissionDto>> GetAllAsync()
    {
        var list = await _unitOfWork.Permissions.GetAllAsync();
        await _kafka.PublishAsync("get");
        return list.Select(p => new PermissionDto
        {
            Id = p.Id,
            EmployeeName = p.EmployeeName,
            EmployeeLastName = p.EmployeeLastName,
            PermissionDate = p.PermissionDate,
            PermissionTypeId = p.PermissionTypeId
        });
    }

    public async Task<PermissionDto?> GetByIdAsync(int id)
    {
        var p = await _unitOfWork.Permissions.GetByIdAsync(id);
        if (p == null) return null;
        await _kafka.PublishAsync("get");

        return new PermissionDto
        {
            Id = p.Id,
            EmployeeName = p.EmployeeName,
            EmployeeLastName = p.EmployeeLastName,
            PermissionDate = p.PermissionDate,
            PermissionTypeId = p.PermissionTypeId
        };
    }

    public async Task<PermissionDto> CreateAsync(PermissionDto dto)
    {
        var permission = new Permission
        {
            EmployeeName = dto.EmployeeName,
            EmployeeLastName = dto.EmployeeLastName,
            PermissionDate = dto.PermissionDate,
            PermissionTypeId = dto.PermissionTypeId
        };

        await _unitOfWork.Permissions.AddAsync(permission);
        await _unitOfWork.CommitAsync();
        await _indexService.IndexAsync(permission);
        await _kafka.PublishAsync("create");

        dto.Id = permission.Id;
        return dto;
    }

    public async Task<PermissionDto?> UpdateAsync(int id, PermissionDto dto)
    {
        var p = await _unitOfWork.Permissions.GetByIdAsync(id);
        if (p == null) return null;

        p.EmployeeName = dto.EmployeeName;
        p.EmployeeLastName = dto.EmployeeLastName;
        p.PermissionDate = dto.PermissionDate;
        p.PermissionTypeId = dto.PermissionTypeId;

        _unitOfWork.Permissions.Update(p);
        await _unitOfWork.CommitAsync();
        await _indexService.IndexAsync(p);
        await _kafka.PublishAsync("modify");

        dto.Id = p.Id;
        return dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _unitOfWork.Permissions.GetByIdAsync(id);
        if (p == null) return false;
        _unitOfWork.Permissions.Remove(p);
        await _unitOfWork.CommitAsync();
        return true;
    }
}
