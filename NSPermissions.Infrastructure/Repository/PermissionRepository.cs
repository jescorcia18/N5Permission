using Microsoft.EntityFrameworkCore;
using NSPermissions.Domain.Entities;
using NSPermissions.Domain.Interfaces;
using NSPermissions.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSPermissions.Infrastructure.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public PermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync() => await _context.Permissions.ToListAsync();

        public async Task<Permission?> GetByIdAsync(int id) => await _context.Permissions.FindAsync(id);

        public async Task AddAsync(Permission permission) => await _context.Permissions.AddAsync(permission);

        public void Update(Permission permission) => _context.Permissions.Update(permission);

        public void Remove(Permission permission) => _context.Permissions.Remove(permission);
    }
}
