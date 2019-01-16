using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
   public interface IBuilderQuaterContractProjectDetails
    {
       IEnumerable<BuilderQuaterContractProjectDetails> GetAllProjectDetailsOfContract(Int64 QuestionId, Int64 BuilderQuaterAdminReportId);
    }
}
