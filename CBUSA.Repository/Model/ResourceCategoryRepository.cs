using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    public class ResourceCategoryRepository : Repository<ResourceCategory>, IResourceCategoryRepository
    {
        public ResourceCategoryRepository(CBUSADbContext Context)
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

        public IEnumerable<ResourceCategory> GetResourceCategoryListForContract(Int64 ContractId)
        {
            var ContractResourceCategory = (from rs in Context.DbResource where (rs.ContractId == ContractId && rs.RowStatusId == (int) RowActiveStatus.Active)
                                            select rs.ResourceCategoryId).Distinct();
            
            var ResourceCategory = (from rc in Context.DbResourceCategory
                                    where (ContractResourceCategory.Contains(rc.ResourceCategoryId) && rc.RowStatusId == (int)RowActiveStatus.Active)
                                    select new
                                    {
                                        ResourceCategoryId = rc.ResourceCategoryId,
                                        ResourceCategoryName = rc.ResourceCategoryName
                                    });

            List<ResourceCategory> ObjResult = new List<ResourceCategory>();
            foreach (var Item in ResourceCategory)
            {
                ObjResult.Add(new ResourceCategory
                {
                    ResourceCategoryId = Item.ResourceCategoryId,
                    ResourceCategoryName = Item.ResourceCategoryName
                });
            }

            return ObjResult;
        }

        //Added on 29.05.2017 against VSO#5518 - To get Resource Category List based on Market as well
        public IEnumerable<ResourceCategory> GetResourceCategoryListForContractMarket(Int64 ContractId, Int64 MarketId)
        {
            var ContractMarketResourceCategory = (from rc in Context.DbResourceCategory
                                                join r in Context.DbResource on rc.ResourceCategoryId equals r.ResourceCategoryId
                                                join rm in Context.DbResourceMarket on r.ResourceId equals rm.ResourceId 
                                                where (r.ContractId == ContractId && rm.MarketId == MarketId
                                                && r.RowStatusId == (int) RowActiveStatus.Active 
                                                && rc.RowStatusId == (int) RowActiveStatus.Active)
                                                select rc.ResourceCategoryId).Distinct();

            var ResourceCategory = (from rc in Context.DbResourceCategory
                                    where (ContractMarketResourceCategory.Contains(rc.ResourceCategoryId) && rc.RowStatusId == (int) RowActiveStatus.Active)
                                    select new
                                    {
                                        ResourceCategoryId = rc.ResourceCategoryId,
                                        ResourceCategoryName = rc.ResourceCategoryName
                                    });

            List<ResourceCategory> ObjResult = new List<ResourceCategory>();
            foreach (var Item in ResourceCategory)
            {
                ObjResult.Add(new ResourceCategory
                {
                    ResourceCategoryId = Item.ResourceCategoryId,
                    ResourceCategoryName = Item.ResourceCategoryName
                });
            }

            return ObjResult;
        }
    }
}
