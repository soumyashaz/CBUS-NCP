using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
    public interface IContractBuilderService
    {

        IEnumerable<ContractBuilder> GetBuilderofContract(Int64 ContractId);

        IEnumerable<ContractBuilder> GetBuilderContractInformation(Int64 ContractId,Int64 BuilderId);
        IEnumerable<ContractBuilder> GetActiveContractsofBuilder(Int64 BuilderId);        
        IEnumerable<ContractBuilder> GetAllContractofBuilder(Int64 BuilderId);
        IEnumerable<ContractBuilder> GetArchiveContractsofBuilder(Int64 BuilderId);
        IEnumerable<ContractBuilder> GetPendingContractsofBuilder(Int64 BuilderId);
        IEnumerable<Contract> GeBuildContract();
        IEnumerable<Contract> GetActiveContractsRegularReporting(Int64 BuilderId);
        IEnumerable<Contract> GetDeclinedContractsofBuilder(Int64 BuilderId, Int64 ContractStatus);
        IEnumerable<Contract> GetActiveOnlyContractsRegularReporting(Int64 BuilderId);
        IEnumerable<Contract> GetActiveOnlyContractsRegularReportingBybuilderJoining(Int64 BuilderId, Int64 QuarterId);
        IEnumerable<ContractBuilder> GetActiveContractsofBuilder(long BuilderId, long QuaterId);
    }
}
