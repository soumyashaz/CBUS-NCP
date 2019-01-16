using System;
using System.Collections.Generic;
using CBUSA.Domain;
namespace CBUSA.Services
{
    public interface IContractServices
    {
        IEnumerable<CBUSA.Domain.Contract> GetContract();
        void SaveContract(CBUSA.Domain.Contract ObjContract, List<string> ObjContractProduct,
            ContractStatusHistory ObjContractStatusHistory, List<Resource> ObjResource);
        bool IsContractNameAvailable(string ContractName);
        bool IsContractLabelAvailable(string ContractLabelName);
        IEnumerable<Resource> GetResourceofDump(string DumpId);

        IEnumerable<Contract> GetActiveContract();
        IEnumerable<Contract> GetActiveContractDescending();
        IEnumerable<Contract> GetArchievedContractDescending();
        Contract GetContract(Int64 ContractId);
        Contract GetBuilderCount(Int64 ContractId);
        void EditContract(CBUSA.Domain.Contract ObjContract, ContractStatusHistory ObjContractStatusHistory);
        void SaveContractProduct(List<string> ObjContractProduct, Int64 ContractId);

        void UpdateContractProduct(List<ContractProduct> ObjContractProduct);
        void UpdateContractBuilder(List<ContractBuilder> ObjContractBuilder);

        int GetMarketCount();

        IEnumerable<Contract> GetActivePendingContract();
        IEnumerable<Contract> GetActivePendingContractList();

        IEnumerable<Builder> GetAssociateBuilderWithContract(Int64 ContractId);
        IEnumerable<Contract> GetNonAssociateContractWithBuilder(Int64 BuilderId,string Flag);

       

    }
}
