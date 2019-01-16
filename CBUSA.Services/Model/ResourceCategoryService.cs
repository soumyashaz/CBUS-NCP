using CBUSA.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository;
namespace CBUSA.Services.Model
{
    public class ResourceCategoryService: IResourceCategoryService
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public ResourceCategoryService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<ResourceCategory> GetResourceCategoryAll()
        {
            return _ObjUnitWork.ResourceCategory.GetAll();
            // return list;
        }

        public ResourceCategory GetResourceCategory(Int64 ResourceCategoryId)
        {
            return _ObjUnitWork.ResourceCategory.Get(ResourceCategoryId);
            // // return list;
            //  return _ObjUnitWork.ResourceCategory.

        }

        public void SaveResourceCategory(ResourceCategory ObjResourceCategory)
        {
            _ObjUnitWork.ResourceCategory.Add(ObjResourceCategory);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void EditResourceCategory(ResourceCategory ObjResourceCategory)
        {
            _ObjUnitWork.ResourceCategory.Update(ObjResourceCategory);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DeleteResourceCategory(ResourceCategory ObjResourceCategory)
        {
            _ObjUnitWork.ResourceCategory.Update(ObjResourceCategory);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public IEnumerable<ResourceCategory> GetResourceCategoryListForContract(Int64 ContractId)
        {
            return _ObjUnitWork.ResourceCategory.GetResourceCategoryListForContract(ContractId);
        }

        public IEnumerable<ResourceCategory> GetResourceCategoryListForContractMarket(Int64 ContractId, Int64 MarketId)
        {
            return _ObjUnitWork.ResourceCategory.GetResourceCategoryListForContractMarket(ContractId, MarketId);
        }
    }
}