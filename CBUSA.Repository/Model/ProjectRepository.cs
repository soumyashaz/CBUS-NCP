using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(CBUSADbContext Context)
            : base(Context)
        {
        }
        public CBUSADbContext Context
        {
            get
            {
                return _Context as CBUSADbContext;
            }
        }
        public IEnumerable<Project> GetBuilderSeletedProjectForQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            // return Context.dbp
            return Context.DbProject.Join(Context.DbBuilderQuaterContractProjectReport, x => x.ProjectId, y => y.ProjectId,
                (x, y) => new { x, y }).Where(z => z.y.BuilderId == BuilderId && z.y.QuaterId == QuaterId && z.y.RowStatusId == (int)RowActiveStatus.Active
                && z.y.ProjectStatusId == (int)EnumProjectStatus.ReportUnit && z.y.ContractId == ContractId).Select(m => m.x);
        }
        // added by angshuman on 29-apr-2017
        public IEnumerable<Project> GetBuilderSeletedProjectForQuaterHistory(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            // return Context.dbp
            return Context.DbProject.Join(Context.DbBuilderQuaterContractProjectReport.Where(x => x.ProjectStatusId == (int)EnumProjectStatus.ReportUnit), x => x.ProjectId, y => y.ProjectId,
                (x, y) => new { x, y }).Where(z => z.y.BuilderId == BuilderId && z.y.QuaterId == QuaterId 
                 && z.y.ContractId == ContractId).Select(m => m.x);
        }
        public IEnumerable<BuilderQuaterContractProjectReport> GetBuilderContractStatusWithContractCurrentProject(Int64 BuilderId ,Int64 ContractId)
        {
            // return Context.dbp
            var GetBuilderContractStatusWithContractProjectReportObj =
                from BQCS in Context.DbBuilderQuarterContractStatus
                join BQCPR in Context.DbBuilderQuaterContractProjectReport
                on BQCS.BuilderQuarterContractStatusId equals BQCPR.BuilderQuarterContractStatusId
                into BQCPRTemp
                from BQCPRLJ in BQCPRTemp.DefaultIfEmpty(new BuilderQuaterContractProjectReport())
                where
               BQCS.BuilderId == BuilderId && BQCS.ContractId == ContractId  && BQCS.RowStatusId == (int)RowActiveStatus.Active
               && BQCPRLJ.Project != null && BQCPRLJ.RowStatusId == (int)RowActiveStatus.Active && BQCPRLJ.ProjectStatusId!=(int)EnumProjectStatus.NeverReport
               && BQCPRLJ.ProjectStatusId != (int)EnumProjectStatus.ReportUnit && BQCS.QuarterContractStatusId != (Int64)QuarterContractStatusEnum.NeverReportForThisContract
                select BQCPRLJ;
            return GetBuilderContractStatusWithContractProjectReportObj;
                             
        }
        public IEnumerable<BuilderQuaterContractProjectReport> GetBuilderContractStatusWithContractPerviousProject(Int64 BuilderId, Int64 ContractId)
        {
            // return Context.dbp
                var GetBuilderContractStatusWithContractProjectReportObj =
                from BQCS in Context.DbBuilderQuarterContractStatus
                join BQCPR in Context.DbBuilderQuaterContractProjectReport
                on BQCS.BuilderQuarterContractStatusId equals BQCPR.BuilderQuarterContractStatusId
                into BQCPRTemp
                from BQCPRLJ in BQCPRTemp.DefaultIfEmpty(new BuilderQuaterContractProjectReport())
                where
               BQCS.BuilderId == BuilderId && BQCS.ContractId == ContractId && BQCS.RowStatusId == (int)RowActiveStatus.Active && BQCS.QuarterContractStatusId!=(Int64)QuarterContractStatusEnum.NeverReportForThisContract
               && BQCPRLJ.Project != null && BQCPRLJ.RowStatusId == (int)RowActiveStatus.Active && BQCPRLJ.ProjectStatusId != (int)EnumProjectStatus.NeverReport
               && BQCPRLJ.ProjectStatusId != (int)EnumProjectStatus.NothingtoReport
                select BQCPRLJ;
            return GetBuilderContractStatusWithContractProjectReportObj;

        }
    }
}
