using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Repository.Interface
{
    public interface IUserInRoleRepository : IRepository<UserInRole>
    {
       bool UserIsAthorishedRole(string SystemRole, int UserId);
    }
}
