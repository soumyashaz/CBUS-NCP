using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    public class ContractStatusRepository : Repository<ContractStatus>, IContractStatusRepository
    {

        public ContractStatusRepository(CBUSADbContext Context)
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


        public IEnumerable<ContractStatus> GetUseContractStatus()
        {
            return Context.DbContractStatus.Join(Context.DbContract, x => x.ContractStatusId, y => y.ContractStatusId, (x, y) => new { x, y }
               ).Where(m => m.x.RowStatusId == (int)RowActiveStatus.Active && (m.y.RowStatusId == (int)RowActiveStatus.Active|| m.y.RowStatusId == (int)RowActiveStatus.Archived))
               .Select(m => m.x);
        }
       
    }
}
