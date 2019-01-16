using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository.Interface;

namespace CBUSA.Repository.Model
{
    class BuilderQuaterContractProjectReportRepository : Repository<BuilderQuaterContractProjectReport>, IBuilderQuaterContractProjectReportRepository
    {
        public BuilderQuaterContractProjectReportRepository(CBUSADbContext Context)
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
        public IEnumerable<BuilderQuaterContractProjectReport> GetBuilderSeletedProjectReportForQuater(Int64 BuilderId, Int64 QuaterId, Int64 ContractId)
        {
            // return Context.dbp
            return Context.DbProject.Join(Context.DbBuilderQuaterContractProjectReport, x => x.ProjectId, y => y.ProjectId,
                (x, y) => new { x, y }).Where(z => z.y.BuilderId == BuilderId && z.y.QuaterId == QuaterId && z.y.RowStatusId == (int)RowActiveStatus.Active
                && z.y.ProjectStatusId == (int)EnumProjectStatus.ReportUnit && z.y.ContractId == ContractId).Select(m => m.y);
        }
    }
}
