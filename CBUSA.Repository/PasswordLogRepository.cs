using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using System.Runtime.Remoting.Contexts;

namespace CBUSA.Repository
{
    public class PasswordLogRepository : Repository<PasswordLog>, IPasswordLogRepository
    {

        public PasswordLogRepository(CBUSADbContext Context)
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
    }
}
