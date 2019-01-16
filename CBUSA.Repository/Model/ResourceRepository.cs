using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    public class ResourceRepository : Repository<Resource>, IResourceRepository
    {

        public ResourceRepository(CBUSADbContext Context)
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

        public bool IsResourceLableUniueInContract(string LableName, string DumpId)
        {
            return Context.DbResource.Where(x => x.Title == LableName && x.DumpId == DumpId).Any();
        }

        public bool IsResourceLabelUniqueWithContract(string LableName, Int64 ContractId)
        {
            return Context.DbResource.Where(x => x.Title == LableName && x.ContractId == ContractId && x.RowStatusId == (int)RowActiveStatus.Active).Any();
        }

        public IEnumerable<Resource> GetResourcebyCategoryMarket(Int64 ContractId, Int64 CategoryId, Int64 MarketId)
        {
            return Context.DbResource.Where(x => x.ContractId == ContractId && x.ResourceCategoryId == CategoryId).
                Join(Context.DbResourceMarket, x => x.ResourceId, y => y.ResourceId, (x, y) => new { x, y })
                .Where(z => z.x.ContractId == ContractId && z.x.ResourceCategoryId == CategoryId && z.y.MarketId == MarketId)
                .Select(m => m.x);


        }

    }
}
