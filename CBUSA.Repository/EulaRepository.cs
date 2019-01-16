using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository
{
    public class EulaRepository : Repository<Eula>, IEulaRepository
    {
        public EulaRepository(CBUSADbContext Context)
            : base(Context)
        {
        }


        public CBUSADbContext Context
        {
            get
            {
                return _Context as CBUSADbContext;
            }
        }

        public string GetRoleWiseEula(int RoleId)
        {
            Eula ObjEula = Context.DbsEula.Where(x => x.RoleId == RoleId).FirstOrDefault();
            return ObjEula != null ? ObjEula.EulaDescription : "";
        }
    }
}
