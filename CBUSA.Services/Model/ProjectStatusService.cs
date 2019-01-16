using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;

namespace CBUSA.Services.Model
{
  public  class ProjectStatusService:IProjectStatusService
    {
       private readonly IUnitOfWork _ObjUnitWork;
       public ProjectStatusService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
       public IEnumerable<ProjectStatus> GetProjectStatus()
       {
           return _ObjUnitWork.ProjectStatus.GetAll();
       }
    }
}
