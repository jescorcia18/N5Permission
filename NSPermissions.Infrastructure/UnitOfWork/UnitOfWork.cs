using NSPermissions.Domain.Interfaces.UnitOfWork;
using NSPermissions.Domain.Interfaces;
using NSPermissions.Infrastructure.Persistence;
using NSPermissions.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSPermissions.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IPermissionRepository Permissions { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Permissions = new PermissionRepository(context);
    }

    public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
}
