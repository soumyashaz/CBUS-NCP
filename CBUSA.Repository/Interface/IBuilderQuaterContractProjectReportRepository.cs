using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Repository.Interface
{
    public interface IBuilderQuaterContractProjectReportRepository : IRepository<BuilderQuaterContractProjectReport>
    {
        IEnumerable<BuilderQuaterContractProjectReport> GetBuilderSeletedProjectReportForQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId);
    }
}
