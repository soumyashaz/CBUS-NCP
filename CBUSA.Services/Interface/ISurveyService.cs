using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
    public interface ISurveyService
    {
        void Flag(bool flag);
        IEnumerable<Survey> GetSurveyAll();
        IEnumerable<Survey> GetSurveyAllActiveAndClose();
        Survey GetSurvey(Int64 SurveyId);
        bool IsSurveyExist(Int64 SurveyId);
        void SaveSurvey(Survey ObjSurvey);
        void UpdateSurvey(Survey ObjSurvey);
        void DeleteSurvey(Survey ObjSurvey);
        IEnumerable<Survey> FindContractSurveys(Int64 ContractId);
        IEnumerable<Survey> FindContractSurveysAll(Int64 ContractId);
        void SaveSurveyMarket(List<Int64> MarketList, Int64 SurveyId);
        void RemoveSurveyMarket(Int64 MarketId, Int64 SurveyId);
        void SaveSurveyEmailSetting(SurveyEmailSetting ObjSurveyEmailSetting, SurveyInviteEmailSetting ObjSurveyInviteEmailSetting,
           SurveyRemainderEmailSetting ObjSurveyRemainderEmailSetting, SurveySaveContinueEmailSetting ObjSurveySaveContinueEmailSetting
           );
        void EditSurveyEmailSetting(SurveyEmailSetting ObjSurveyEmailSetting, SurveyInviteEmailSetting ObjSurveyInviteEmailSetting,
            SurveyRemainderEmailSetting ObjSurveyRemainderEmailSetting, SurveySaveContinueEmailSetting ObjSurveySaveContinueEmailSetting
            );
        SurveyEmailSetting GetSurveyEmailSetting(Int64 SurveyId);
        void SaveSurveyInviteEmailSetting(SurveyInviteEmailSetting ObjSurveyInviteEmailSetting);
        void UpdateSurveyInviteEmailSetting(SurveyInviteEmailSetting ObjSurveyInviteEmailSetting);
        SurveyInviteEmailSetting GetSurveyInviteEmailSetting(string DumpId);
        void SaveSurveyRemainderEmailSetting(SurveyRemainderEmailSetting ObjSurveyRemainderEmailSetting);
        void UpdateSurveyRemainderEmailSetting(SurveyRemainderEmailSetting ObjSurveyRemainderEmailSetting);
        SurveyRemainderEmailSetting GetSurveyRemainderEmailSetting(string DumpId);
        void SaveSurveySaveContinueEmailSetting(SurveySaveContinueEmailSetting ObjSurveySaveContinueEmailSetting);
        void UpdateSurveySaveContinueEmailSetting(SurveySaveContinueEmailSetting ObjSurveySaveContinueEmailSetting);
        SurveySaveContinueEmailSetting GetSurveySaveContinueEmailSetting(string DumpId);


        IEnumerable<SurveyBuilder> GetSurveyBuilder(Int64 ContractId);
        SurveyEmailSetting GetSurveyEmail(Int64 SurveyId);

        SurveyInviteEmailSetting GetSurveyInviteEmailSettingBySurveySetting(Int64 SurveySettingId);

        IEnumerable<Survey> GetContractSurvey(Int64 ContractId);
        IEnumerable<Survey> GetContractEnrolledSurvey(Int64 ContractId);
        bool CopySurvey(Int64 ContractId, Int64 SurveyId, bool? IsNcp);
        bool IsEnrollmentAvailable(Int64 ContractId);
        IEnumerable<Survey> GetSurveyAllArchive();
        IEnumerable<Survey> FindContractSurveysAllArchive(Int64 ContractId);
        IEnumerable<Survey> GetNcpSurveyAllArchive();
        IEnumerable<Survey> FindContractNcpSurveysAllArchive(Int64 ContractId);
        IEnumerable<Survey> GetNcpSurveyAll();

        IEnumerable<Survey> FindContractNcpSurveysAll(Int64 ContractId);
        IEnumerable<SurveyResult> GetBuilderSurveyResult(Int64 SurveyId, Int64 BuilderId);



        //  IEnumerable<Survey> Get(Int64 ContractId);
        string[] GetQuaterAll();
        long[] GetYearAll();
        bool[] IsNcpSurvey(Int64 SurveyId);
        bool IsNcpSurveyAvailable(string Quater, string Year, Int64 ContractId);

        void SaveSurveyResult(Int64 Surveyid, Int64 BuilderId, bool IsSurveyComplete, List<SurveyResult> ObjSurveyResult, string ServerFilePath);
        IEnumerable<SurveyResult> GetQuestionWiseBuilderSurveyResult(Int64 SurveyId, Int64 BuilderId, Int64 QuestionId);
       // IEnumerable<dynamic> GetSurveyResponse(Int64 SurveyId);
      //  IEnumerable<dynamic> GetSurveyResponse(Int64 SurveyId, Int64 BuilderId);

        bool IsBuilderAuthorizedToAcessSurvey(Int64 SurveyId, Int64 BuilderId);
        bool IsBuilderAllraedyCompleteSurvey(Int64 SurveyId, Int64 BuilderId);

        Survey GetEditResponseSurvey(Int64 SurveyId);
        void EditSurveyResultByAdmin(Int64 SurveyId, Int64 BuilderId, List<SurveyResult> ObjSurveyResult, string ServerFilePath);
        IEnumerable<SurveyMarket> GetSurveyMarket(Int64 SurveyId);
        int GetCompleteedBuilderNCP(Int64 SurveyId);
        int GetInCompleteedBuilderNCP(Int64 SurveyId);
        
        IEnumerable<Builder> BuilderAllreadyGetInvitationForSurvey(Int64 SurveyId,bool IsEnrollmentSurvey);
        IEnumerable<Survey> GetNcpSurveyAllActiveAndClose();
        IEnumerable<Survey> FindContractNcpSurveysAllByQuarter(Int64 ContractId, string Quarter, string Year);
        //IEnumerable<SurveyResponseEditStatus> GetNCPSurveyResponseEditStatus(Int64 BuilderId, Int64 QuaterId, Int64? ContractId);
        //bool GetNCPSurveyResponseEditPermission(Int64 BuilderId, Int64 ContractId, Int64 QuarterId);
        //string SaveNCPSurveyResponseEditPermission(SurveyResponseEditStatus ObjModel);
    }
}
