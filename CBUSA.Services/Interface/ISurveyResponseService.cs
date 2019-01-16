using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
namespace CBUSA.Services.Interface
{
    public interface ISurveyResponseService
    {
        SurveyResult GetSurveyResult(Int64 SurveyResultId);
        void UpdateSurveyResult(SurveyResult ObjSurveyResult, bool DisposeConn = false);
        IEnumerable<SurveyBuilder> GetSurveyBuilderId(Int64 SurveyId, Int64 BuilderId);
        SurveyBuilder GetSurveyBuilder(Int64 SurveyBuilderId);
        void UpdateSurveyBuilder(SurveyBuilder ObjSurveyBuilder, bool DisposeConn = false);
        IEnumerable<SurveyBuilder> GetParticipatedBuilder(Int64 SurveyId, int SurveyCompleteFilter);
        IEnumerable<dynamic> GetParticipatedBuilderActive(Int64 SurveyId, int SurveyCompleteFilter);
        IEnumerable<SurveyResponse> GetBuilderSurveyResult(Int64 SurveyId, Int64 BuilderId);
        IEnumerable<SurveyResponse> GetBuilderSurveyResponseFiltered(Int64 SurveyId, Int64 BuilderId, List<Int64> QuestionFilter);
        // - newly added - angshuman on 17/09/2016
        IEnumerable<dynamic> GetParticipatedBuilderNCP(Int64 SurveyId, int SurveyCompleteFilter);
        bool CheckNCPSurvey(Int64 SurveyId);
        IEnumerable<SurveyResponseNCP> GetBuilderSurveyResponseNCP(Int64 SurveyId, Int64 BuilderId);
        //IEnumerable<dynamic> GetBuilderSurveyResponseNCPNew(Int64 SurveyId, List<Int64> QuestionFilter);
        string GetContractQuater(Int64 SurveyId);
        IEnumerable<SurveyResponseNCP> GetBuilderSurveyResponseNCPFiltered(Int64 SurveyId, Int64 BuilderId, List<Int64> QuestionFilter);
        IEnumerable<SurveyResult> GetSurveyResultId(Int64 SurveyId, Int64 BuilderId);
        BuilderQuaterContractProjectDetails GetBuilderQuaterProjectDetails(Int64 ObjBuilderQuaterContractProjectDetailsId);
        void UpdateBuilderQuaterContractProjectDetails(BuilderQuaterContractProjectDetails ObjBuilderProjects, bool DisposeConn = false);
        IEnumerable<BuilderQuaterContractProjectReport> GetBuilderQuaterContractProjectReport(Int64 ProjectId, Int64 BuilderId, Int64 QuaterId, Int64 ContractId);
        BuilderQuaterAdminReport GetBuilderQuaterAdminReportId(Int32 BuilderQuaterAdminReportId);
        void UpdateBuilderQuaterAdminReport(BuilderQuaterAdminReport ObjBuilderReport, bool DisposeConn = false);
        IEnumerable<Builder> GetBuilderDetails(Int64 BuilderId);
        IEnumerable<string> GetBuilderMrketName(Int64 MarketId);
    }
}
