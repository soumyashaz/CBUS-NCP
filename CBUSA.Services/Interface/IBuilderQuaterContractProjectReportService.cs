using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
namespace CBUSA.Services.Interface
{
    public interface IBuilderQuaterContractProjectReportService
    {
        void SaveProjectReport(BuilderQuaterContractProjectReport objProject, bool disposeDbContext = true);        
        void UpdateProjectReport(BuilderQuaterContractProjectReport objProject);
        void UpdateMultipleProjectReport(BuilderQuaterContractProjectReport objProject);
        void GetDispose();
        IEnumerable<BuilderQuaterContractProjectReport> CheckExistProjectAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId, Int64 ProjectId);
        IEnumerable<BuilderQuaterContractProjectReport> CheckExistContractAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId);
        IEnumerable<BuilderQuaterContractProjectReport> CheckCompleteContractAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId);
        IEnumerable<BuilderQuaterContractProjectReport> CheckExistAllContractAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId);
        IEnumerable<BuilderQuaterContractProjectReport> CheckAllContractReportSubmit(Int64 BuilderId, Int64 QuaterId);
        IEnumerable<BuilderQuaterContractProjectReport> CheckAllContractForBuilderQuarter(Int64 BuilderId, Int64 QuaterId);
        void Flag(bool flag);
        IEnumerable<BuilderQuaterContractProjectReport> GetSelectedProjectForQuater(Int64 ContractId, Int64 BuilderId, Int64 QuaterId);
        IEnumerable<BuilderQuaterContractProjectReport> GetAllProjectCountForQuater(Int64 ContractId, Int64 BuilderId, Int64 QuaterId);
        IEnumerable<BuilderQuaterContractProjectReport> GetRepotDetails(Int64 BuilderId, Int64 ContractId);
        IEnumerable<BuilderQuaterContractProjectReport> GetActiveRepotDetails(Int64 BuilderId, Int64 ContractId);
        IEnumerable<BuilderQuaterContractProjectReport> GetActiveQuaterRepotDetails(Int64 BuilderId, Int64 ContractId, Int64 QuaterId);
        IEnumerable<BuilderQuaterContractProjectReport> GetRepotDetailsofAllContract(Int64 BuilderId);
        IEnumerable<BuilderQuaterContractProjectReport> GetRepotDetailsofSpecificAdminContractId(Int64 BuilderAdminContractId);
        //        void SaveBuilderProjectResult(Int64 ContractId, Int64 BuilderId, Int64 QuaterId, List<Int64> ProjectList, List<BuilderQuaterContractProjectDetails> ObjReport, string ServerFilePath, bool AddToSurveyResponseHistory = false);
        void SaveBuilderProjectResult(Int64 ContractId, Int64 BuilderId, Int64 QuaterId, List<Int64> ProjectList, List<BuilderQuaterContractProjectDetails> ObjReport, string ServerFilePath);
        IEnumerable<BuilderQuaterContractProjectReport> GetBuilderSeletedProjectReportForQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId);
        IEnumerable<BuilderQuaterContractProjectReport> GetBuilderCurrentQuaterSelectedProject(Int64 BuilderId, Int64 ContractId, Int64 QuaterId);
        BuilderQuaterContractProjectReport GetBuilderQuaterContractProjectReport(Int64 BuilderId, Int64 ContractId, Int64 QuaterId, Int64 ProjectId);
        void SaveBuilderProjectStatus(List<BuilderQuaterContractProjectReport> objProject);
        void UpdateBuilderContractProjectStatus(List<BuilderQuaterContractProjectReport> objProject, bool disposeDbContext = true);
        void EditBuilderProjectResultByAdmin(Int64 ContractId, Int64 BuilderId, Int64 QuaterId,
        Int64 ProjectId, List<BuilderQuaterContractProjectDetails> ObjReport, string ServerFilePath);
        void UpdateBuilderProjectStatus(Int64 BuilderId, Int64 ContractId, Int64 QuaterId, Int64 ProjectId);
        void UpdateBuilderMultipleProjectStatus(Int64 BuilderId, Int64 ContractId, Int64 QuaterId, List<Int64> ListProjectId);
        IEnumerable<BuilderQuaterContractProjectReport> CheckExistAllContractAgainstBuilderQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId, Int64 ProjectId);
        IEnumerable<BuilderQuaterContractProjectReport> CheckExistProjectAgainstBuilderQuaterContract(Int64 BuilderId, Int64 QuaterId, Int64 ContractId, Int64 ProjectId);
        void ForceDisposeDbContext();
        IEnumerable<BuilderQuaterContractProjectReport> GetLatestContractAgainstBuilderProject(Int64 BuilderId, Int64 ProjectId);
        IEnumerable<BuilderQuaterContractProjectReport> CheckCompleteBuilderQuaterContractProjectReport(Int64 BuilderId, Int64 QuaterId, Int64 ContractId);
        IEnumerable<BuilderQuarterContractStatus> GetReportDetails(Int64 BuilderId, Int64 ContractId);
        void SaveProjectReportInBulk(List<BuilderQuaterContractProjectReport> objProject);
    }
}
