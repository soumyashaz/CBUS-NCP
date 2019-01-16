using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Repository
{
    public class ResourceMarketRepository : Repository<ResourceMarket>, IResourceMarketRepository
    {
        public ResourceMarketRepository(CBUSADbContext Context)
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
