using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(CBUSADbContext Context)
            : base(Context)
        {
        }
    
    }
}
