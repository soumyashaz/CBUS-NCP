using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services.Interface
{
    public interface IContractComplianceService
    {
        IEnumerable<ContractCompliance> GetContractComplianceAll();
       
        ContractCompliance GetContractCompliance(Int64 ContractComplianceId);

        void SaveContractCompliance(ContractCompliance ObjContractCompliance);
        void UpdateContractCompliance(ContractCompliance ObjContractCompliance);
        void DeleteContractCompliance(ContractCompliance ObjContractCompliance);

        //   bool IsContractEstimateComplianceExist(Int64 ContractId);
        //   bool IsContractActualComplianceExist(Int64 ContractId);

        ContractCompliance ContractEstimateCompliance(long ContractId);
        ContractCompliance ContractActualCompliance(long ContractId);

        IEnumerable<ContractComplianceBuilder> GetContractBuilderComplianceAll(Int64 ContractId);
        ContractComplianceBuilder GetContractBuilderCompliance(Int64 ContractId, Int64 BuilderId);
      
        void SaveContractComplianceBuilder(List<ContractComplianceBuilder> ObjInsertList, List<ContractComplianceBuilder> ObjUpdateList);

        decimal[] GetBuilderComplianceFactor(Int64 ContractId, Int64 BuilderId, bool IsOverrideConsider);
        IEnumerable<ContractCompliance> GetEstimatedValueCompliance(Int64 ContractId);
        IEnumerable<ContractCompliance> GetActualValueCompliance(Int64 ContractId);
        ContractCompliance ContractActualComplianceWithQuater(long ContractId,long QuaterId);

        IEnumerable<ContractCompliance> GetActualValueCompliancePerQuater(Int64 ContractId, Int64 QuaterId);
        IEnumerable<ContractComplianceBuilder> GetContractBuilderComplianceNew(Int64 ContractId, Int64 BuilderId);
        decimal GetBuilderActualValueWithQuater(Int64 BuilderId, Int64 ContractId, List<Int64> QuestionidList, List<Int64> QuaterIdList);
    }
}
