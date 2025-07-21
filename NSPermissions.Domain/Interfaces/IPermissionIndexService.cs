using NSPermissions.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSPermissions.Domain.Interfaces
{
    public interface IPermissionIndexService
    {
        Task IndexAsync(Permission permission);
    }

}
