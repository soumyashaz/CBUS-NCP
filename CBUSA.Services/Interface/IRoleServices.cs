using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services.Interface
{
    public interface IRoleServices
    {
        IEnumerable<Role> GetRole();
        Role FindRole(Int32 RoleId);
    }
}
