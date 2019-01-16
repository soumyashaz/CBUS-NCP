using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using System;

namespace CBUSA.Services.Interface
{
    public interface IProjectService
    {
        void SaveProject(Project ObjProject, bool disposeDbContext = true);
        void Flag(bool flag);
        IEnumerable<Project> BuilderProjectList(Int64 BuilderId);
        IEnumerable<dynamic> BuilderProjectListUsingProc(Int64 BuilderId, int RowStatusId);
        IEnumerable<Project> ProjectList();
        IEnumerable<Project> CopyProject(Int64 ProjectId);

        void UpdateProject(Project ObjProject, bool disposeDbContext = true);
        IEnumerable<Project> GetBuilderActiveProject(Int64 BuilderId);
        IEnumerable<Project> GetBuilderSeletedProjectForQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId);
        IEnumerable<Project> GetBuilderSeletedProjectForQuaterHistory(Int64 BuilderId, Int64 QuaterId, Int64 ContractId);

        int GetBuilderProjectCount(Int64 BuilderId);
        #region Add ProjectStatus
        IEnumerable<Project> GetBuilderProject(Int64 ContractId, Int64 BuilderId,List<Int64> PreviousQuater);
        IEnumerable<Project> GetContractBuilderCurrentQuaterProject(Int64 ContractId, Int64 BuilderId, List<Int64> PreviousQuater, Int64 FilterProjectId);
        IEnumerable<Project> GetContractBuilderPriviousQuaterProject(Int64 ContractId, Int64 BuilderId, List<Int64> PreviousQuater);
        #endregion

        void SaveBuilderProject(List<Project> ObjProjectVM);
        IEnumerable<Project> GetSelectedProjectbyProjectList(List<Int64> ProjectList);
        //IEnumerable<Project> GetContractBuilderPriviousQuaterProjectNew(Int64 ContractId, Int64 BuilderId, List<Int64> PreviousQuater);
        //IEnumerable<Project> GetSelectedProjectbyProjectList(Int64 BuilderId, Int64 ContractId);
        IEnumerable<dynamic> GetDataIntoListQuery(string query);
        string DeleteDataUsingADONET(string query);
        void CheckProjectReportStatus(Int64 BuilderId, Int64 QuaterId, Int64 ContractId);
    }
}
