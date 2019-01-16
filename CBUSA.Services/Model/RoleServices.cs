using CBUSA.Repository;
using CBUSA.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Model
{
    public class RoleServices : IRoleServices
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public RoleServices(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<Role> GetRole()
        {
           return _ObjUnitWork.Role.GetAll();
        }
        public Role FindRole(Int32 RoleId)
        {

            return _ObjUnitWork.Role.Get(RoleId);
        }
    }
}
