using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
    public interface IResourceCategoryService
    {
        IEnumerable<ResourceCategory> GetResourceCategoryAll();
        ResourceCategory GetResourceCategory(Int64 ResourceCategoryId);
        void SaveResourceCategory(ResourceCategory ObjResourceCategory);
        void EditResourceCategory(ResourceCategory ObjResourceCategory);
        void DeleteResourceCategory(ResourceCategory ObjResourceCategory);
        IEnumerable<ResourceCategory> GetResourceCategoryListForContract(Int64 ContractId);
        IEnumerable<ResourceCategory> GetResourceCategoryListForContractMarket(Int64 ContractId, Int64 MarketId);        
    }
}
