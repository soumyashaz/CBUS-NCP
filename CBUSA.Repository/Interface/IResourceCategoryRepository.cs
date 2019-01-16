using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Repository.Interface
{
    public interface IResourceCategoryRepository : IRepository<ResourceCategory>
    {
        IEnumerable<ResourceCategory> GetResourceCategoryListForContract(Int64 ContractId);
        IEnumerable<ResourceCategory> GetResourceCategoryListForContractMarket(Int64 ContractId, Int64 MarketId);
    }
}
