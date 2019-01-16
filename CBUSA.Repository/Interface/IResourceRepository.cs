using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Repository.Interface
{
    public interface IResourceRepository : IRepository<Resource>
    {
        bool IsResourceLableUniueInContract(string LableName, string DumpId);
        bool IsResourceLabelUniqueWithContract(string LableName, Int64 ContractId);
        IEnumerable<Resource> GetResourcebyCategoryMarket(Int64 ContractId, Int64 CategoryId, Int64 MarketId);
    }
}
