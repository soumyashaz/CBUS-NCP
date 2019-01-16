using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;
using System.Data.SqlClient;

namespace CBUSA.Repository.Model
{
    public class ConstructFormulaMarketRepository : Repository<ConstructFormulaMarket>, IConstructFormulaMarketRepository
    {
        public ConstructFormulaMarketRepository(CBUSADbContext Context)
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
