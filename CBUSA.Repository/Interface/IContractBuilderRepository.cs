using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
namespace CBUSA.Repository.Interface
{
    public interface IContractBuilderRepository : IRepository<ContractBuilder>
    {
        IEnumerable<Contract> GetUseContractList();
        IEnumerable<Contract> GetActiveContractsRegularReporting(Int64 BuilderId);

        IEnumerable<ContractBuilder> GetBuilderArchiveContract(Int64 BuilderId);

        IEnumerable<Contract> GetBuilderDeclinedContract(Int64 BuilderId, Int64 ContractStatus);
        IEnumerable<Contract> GetActiveOnlyContractsRegularReporting(Int64 BuilderId);

        IEnumerable<Contract> GetActiveOnlyContractsRegularReportingBybuilderJoining(Int64 BuilderId, Int64 QuarterId);
    }
}
