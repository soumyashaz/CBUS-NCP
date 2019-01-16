using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Repository.Interface
{
    public interface ISurveyResultRepository : IRepository<SurveyResult>
    {
        IEnumerable<dynamic> GetSurveyResponse(Int64 SurveyId);
        IEnumerable<SurveyResponse> GetBuilderSurveyResponse(Int64 SurveyId, Int64 BuilderId);
        IEnumerable<SurveyResponse> GetBuilderSurveyResponseFiltered(Int64 SurveyId, Int64 BuilderId, List<Int64> QuestionFilter);
        // added newly - angshuman on 17/09/2016
        IEnumerable<dynamic> GetParticipatedBuilderActive(Int64 SurveyId, int SurveyCompleteFilter);
        IEnumerable<dynamic> GetParticipatedBuilderNCP(Int64 SurveyId, int SurveyCompleteFilter);
        bool CheckNCPSurvey(Int64 SurveyId);
        IEnumerable<SurveyResponseNCP> GetBuilderSurveyResponseNCP(Int64 SurveyId, Int64 BuilderId);
        //IEnumerable<dynamic> GetBuilderSurveyResponseNCPNew(Int64 SurveyId, List<Int64> QuestionFilter);
        string GetContractQuater(Int64 SurveyId);
        IEnumerable<SurveyResponseNCP> GetBuilderSurveyResponseNCPFiltered(Int64 SurveyId, Int64 BuilderId, List<Int64> QuestionFilter);
    }
}
