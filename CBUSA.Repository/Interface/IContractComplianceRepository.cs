using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
namespace CBUSA.Repository.Interface
{
    public interface IContractComplianceRepository : IRepository<ContractCompliance>
    {
        //  double GetBuilderTotalActualValue(Int64 BuilderId, Int64 ContractId, List<Int64> QuestionidList);
        decimal GetBuilderTotalActualValue(Int64 BuilderId, Int64 ContractId, List<Int64> QuestionidList);
        decimal GetBuilderActualValueWithQuater(Int64 BuilderId, Int64 ContractId, List<Int64> QuestionidList, List<Int64> QuaterIdList);
    }


    public interface IContractComplianceBuilderRepository : IRepository<ContractComplianceBuilder>
    {
    }
}