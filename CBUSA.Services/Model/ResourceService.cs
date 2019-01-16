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
    public class ResourceService : IResourceService
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public ResourceService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<Resource> GetResourceAll()
        {
            return _ObjUnitWork.Resource.GetAll();
            // return list;
        }

        public Resource GetResource(Int64 ResourceId)
        {
            return _ObjUnitWork.Resource.Get(ResourceId);
            // // return list;
            //  return _ObjUnitWork.Resource.

        }

        public void SaveResource(Resource ObjResource, List<ResourceMarket> ResourceMarketList)
        {
            _ObjUnitWork.Resource.Add(ObjResource);
            foreach (var Itme in ResourceMarketList)
            {
                _ObjUnitWork.ResourceMarket.Add(Itme);
            }
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void UpdateResource(Resource ObjResource, List<ResourceMarket> ResourceMarketList)
        {
            List<ResourceMarket> HistoryList = _ObjUnitWork.ResourceMarket.Find(x => x.ResourceId == ObjResource.ResourceId).ToList();
            var AddList = ResourceMarketList.Select(x => x.MarketId).Except(HistoryList.Select(y => y.MarketId)).ToList();
            var DeletedList = HistoryList.Select(x => x.MarketId).Except(ResourceMarketList.Select(y => y.MarketId)).ToList();
            foreach (var Item in ResourceMarketList)
            {
                if (AddList.Contains(Item.MarketId))
                {
                    Item.ResourceId = ObjResource.ResourceId;
                    _ObjUnitWork.ResourceMarket.Add(Item);
                }

            }

            if (ResourceMarketList.Count > 0)
            {
                foreach (var Item in HistoryList)
                {
                    if (DeletedList.Contains(Item.MarketId))
                    {

                        _ObjUnitWork.ResourceMarket.Remove(Item);
                    }
                }
            }

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void EditResource(Resource ObjResource)
        {
            _ObjUnitWork.Resource.Update(ObjResource);
            List<ResourceMarket> HistoryList = _ObjUnitWork.ResourceMarket.Find(x => x.ResourceId == ObjResource.ResourceId).ToList();
            foreach (var Item in HistoryList)
            {
                _ObjUnitWork.ResourceMarket.Remove(Item);
            }
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DeleteResource(Resource ObjResource)
        {
            _ObjUnitWork.Resource.Update(ObjResource);
            List<ResourceMarket> HistoryList = _ObjUnitWork.ResourceMarket.Find(x => x.ResourceId == ObjResource.ResourceId).ToList();
            foreach (var Item in HistoryList)
            {
                _ObjUnitWork.ResourceMarket.Remove(Item);
            }
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public bool IsResourceLableUniueInContract(string LableName, string DumpId)
        {
            return _ObjUnitWork.Resource.IsResourceLableUniueInContract(LableName, DumpId);
        }


        public IEnumerable<Resource> GetResourceofDump(string DumpId)
        {
            return _ObjUnitWork.Resource.Search(x => x.DumpId == DumpId);
        }

        public bool IsResourceLableUniueWithContract(string LableName, Int64 ContractId)
        {
            return _ObjUnitWork.Resource.IsResourceLabelUniqueWithContract(LableName, ContractId);
        }

        public IEnumerable<Resource> GetResourcebyCategory(Int64 ContractId, Int64 CategoryId)
        {
            return _ObjUnitWork.Resource.Search(x => x.RowStatusId == (int)RowActiveStatus.Active && x.ResourceCategoryId == CategoryId && x.ContractId == ContractId);
        }

        //// Added by Rabi on 9 th mar 2017
        public IEnumerable<Resource> GetResourcebyCategoryMarket(Int64 ContractId, Int64 CategoryId, Int64 MarketId) 
        {
            return _ObjUnitWork.Resource.GetResourcebyCategoryMarket(ContractId, CategoryId, MarketId);
        }

        public IEnumerable<ResourceMarket> GetResourceMarket(Int64 ResourceId)
        {
            return _ObjUnitWork.ResourceMarket.Search(x => x.ResourceId == ResourceId);
        }
    }
}