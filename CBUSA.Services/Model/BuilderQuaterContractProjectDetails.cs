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
    public class BuilderQuaterContractProjectDetails : IBuilderQuaterContractProjectDetails
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public BuilderQuaterContractProjectDetails(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        
            
        }
       //public IEnumerable<BuilderQuaterContractProjectDetails> GetAllProjectDetailsOfContract(Int64 QuestionId, Int64 BuilderQuaterAdminReportId)
       // {
       //    return _ObjUnitWork.BuilderQuaterContractProjectDetails.Search(x=>x.QuestionId==QuestionId && x.BuilderQuaterContractProjectReportId==BuilderQuaterAdminReportId)
       //     return _ObjUnitWork.BuilderQuaterContractProjectDetails.Search(x => x.QuestionId ==
       //         QuestionId && x.BuilderQuaterContractProjectReportId == BuilderQuaterAdminReportId && x.RowStatusId==(int)RowActiveStatus.Active);
       // }
      
    }
}
