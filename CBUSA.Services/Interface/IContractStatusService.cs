using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
namespace CBUSA.Services.Interface
{
    public interface IContractStatusService
    {
        IEnumerable<ContractStatus> GetContractStatusAll();
        ContractStatus GetContractStatus(Int64 ContractStatusId);
        void SaveContractStatus(ContractStatus ObjContractStatus);
        void EditContractStatus(ContractStatus ObjContractStatus);
        void DeleteContractStatus(ContractStatus ObjContractStatus);
        IEnumerable<ContractStatus> GetUseContractStatus();


    }
}
