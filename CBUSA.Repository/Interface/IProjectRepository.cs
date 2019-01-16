using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Repository.Interface
{
    public interface IProjectRepository:IRepository<Project>
    {
        IEnumerable<Project> GetBuilderSeletedProjectForQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId);
        // added by angshuman on 29-apr-2017
        IEnumerable<Project> GetBuilderSeletedProjectForQuaterHistory(Int64 BuilderId, Int64 QuaterId, Int64 ContractId);
        
        IEnumerable<BuilderQuaterContractProjectReport> GetBuilderContractStatusWithContractCurrentProject(Int64 BuilderId, Int64 ContractId);
        IEnumerable<BuilderQuaterContractProjectReport> GetBuilderContractStatusWithContractPerviousProject(Int64 BuilderId, Int64 ContractId);
    }
}
