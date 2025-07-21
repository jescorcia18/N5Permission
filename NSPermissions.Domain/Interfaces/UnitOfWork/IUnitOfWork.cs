using NSPermissions.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSPermissions.Domain.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    IPermissionRepository Permissions { get; }
    Task<int> CommitAsync();
}
