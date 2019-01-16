using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository;
using CBUSA.Services.Interface;

namespace CBUSA.Services.Model
{
    public class ProjectReportStatusService:IProjectReportStatusService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public ProjectReportStatusService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public IEnumerable<ProjectReportStatus> GetProjectReportStatus()
        {
            return _ObjUnitWork.ProjectReportStatus.GetAll();
        }

      
    }
}
