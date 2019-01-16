using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
namespace CBUSA.Services.Interface
{
    public interface IBuilderQuaterContractProjectDetailsService
    {
        IEnumerable<BuilderQuaterContractProjectDetails> GetAllProjectDetailsOfContract(Int64 QuestionId, Int64 BuilderQuaterAdminReportId);
        IEnumerable<BuilderQuaterContractProjectDetails> GetProjectDetailsForBuilderQuaterContractProjectReport(Int64 BuilderQuaterContractProjectReportId);
        IEnumerable<dynamic> GetProjectDetailsForBuilderQuaterContractProjectReportByQuater(Int64 QuaterID);
        void AddProjectReportDetails(BuilderQuaterContractProjectDetails objProjectDetails, bool disposeDbContext = true);
        void UpdateProjectReportDetails(BuilderQuaterContractProjectDetails objProjectDetails, bool disposeDbContext = true);
    }

}
