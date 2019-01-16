using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;


namespace CBUSA.Services.Interface
{
    public interface IContractRebateService
    {


        IEnumerable<ContractRebate> GetContractReabteAll();
        ContractRebate GetContractReabte(Int64 ContractReabteId);
        void SaveContractReabte(ContractRebate ObjContractReabte);
        void EditContractReabte(ContractRebate ObjContractReabte);
        void DeleteContractReabte(ContractRebate ObjContractReabte);
        void SaveContractRebate(List<ContractRebate> ObjContractRebate);

        IEnumerable<ContractRebateBuilder> GetContactBuilderRebateAll(Int64 ContractId, Int64 BuilderId);
        void OverrideContractRebate(ContractRebateBuilder ObjContractRebateBuilder);

    }
}
