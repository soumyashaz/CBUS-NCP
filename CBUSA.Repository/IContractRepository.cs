using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository
{
    public interface IContractRepository : IRepository<Contract>
    {
        //System.Collections.Generic.IEnumerable<Contract> GetContract();
        //void SaveContact(Contract ObjContract);

        bool IsContractNameAvailable(string ContractName);
        bool IsContractLabelAvailable(string ContractLabelName);
        IEnumerable<Contract> GetActiveContract();
        IEnumerable<Contract> GetPendingContract();
        IEnumerable<Contract> GetActiveContractDescending();
        IEnumerable<Contract> GetArchivedontractDescending();
        IEnumerable<Builder> GetAssocaitedBuilderWithContract(Int64 ContractId);
        IEnumerable<Contract> GetNonAssociatedContract(Int64 BuilderId, string Flag);

    }
}