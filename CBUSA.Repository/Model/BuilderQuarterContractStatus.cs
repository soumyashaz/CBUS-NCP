using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;
namespace CBUSA.Repository.Model
{
    public class BuilderQuarterContractStatusRepository : Repository<BuilderQuarterContractStatus>, IBuilderQuarterContractStatusRepository
    {
        public BuilderQuarterContractStatusRepository(CBUSADbContext Context)
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
