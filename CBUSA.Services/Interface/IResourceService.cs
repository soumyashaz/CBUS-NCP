using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
    public interface IResourceService
    {
        IEnumerable<Resource> GetResourceAll();
        Resource GetResource(Int64 ResourceId);
        void SaveResource(Resource ObjResource, List<ResourceMarket> ResourceMarketList);
        void EditResource(Resource ObjResource);
        void DeleteResource(Resource ObjResource);

        bool IsResourceLableUniueInContract(string LableName, string DumpId);

        IEnumerable<Resource> GetResourceofDump(string DumpId);
        void UpdateResource(Resource ObjResource, List<ResourceMarket> ResourceMarketList);
        bool IsResourceLableUniueWithContract(string LableName, Int64 ContractId);
        IEnumerable<CBUSA.Domain.Resource> GetResourcebyCategory(Int64 ContractId, Int64 CategoryId);
        IEnumerable<ResourceMarket> GetResourceMarket(Int64 ResourceId);
        IEnumerable<Resource> GetResourcebyCategoryMarket(Int64 ContractId, Int64 CategoryId, Int64 MarketId); // Added by Rabi on 9 th mar 2017
    }
}
