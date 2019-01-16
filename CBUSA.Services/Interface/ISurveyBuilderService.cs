using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
namespace CBUSA.Services.Interface
{
   public interface ISurveyBuilderService
    {
        IEnumerable<SurveyBuilder> FindInCompleteSurveyBuilderBySurveyId(Int64 SurveyId);
        IEnumerable<SurveyBuilder> FindInCompleteSurveyBySurveyIdBuilderId(Int64 SurveyId,Int64 BuilderId);
      
        IEnumerable<SurveyBuilder> FindCompleteSurveyBuilderBySurveyId(Int64 SurveyId);
        IEnumerable<SurveyBuilder> FindExistSurveyBuilderBySurveyId(Int64 SurveyId);
        IEnumerable<SurveyBuilder> FindSurveyOfBuilder(Int64 SurveyId,Int64 BuilderId);
        IEnumerable<SurveyBuilder> FindCompleteBuilderSurvey(Int64 SurveyId);
        IEnumerable<SurveyBuilder> FindInCompleteBuilderSurvey(Int64 SurveyId);
    }
}
