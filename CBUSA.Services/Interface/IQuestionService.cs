using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
    public interface IQuestionService
    {

        IEnumerable<QuestionType> GetQuestionTypeAll();
        IEnumerable<TextBoxType> GetTextBoxTypeAll();

        IEnumerable<Question> GetQuestionAll();
        Question GetQuestion(Int64 QuestionId);
        Question GetQuestion(Int64 QuestionId, Int64 SurveyId);

        void SaveQuestion(Question ObjQuestion);
        void EditQuestion(Question ObjQuestion);
        void DeleteQuestion(Question ObjQuestion);

        void SaveTextBoxQuestion(Question ObjQuestion, QuestionTextBoxSetting ObjQuestionTextBoxSetting);
        void SaveDropListQuestion(Question ObjQuestion, List<QuestionDropdownSetting> ObjQuestionDropdownSetting);
        void SaveGridQuestion(Question ObjQuestion, QuestionGridSetting ObjQuestionGridSetting, List<QuestionGridSettingHeader> ObjQuestionGridSettingHeader);


        void DeleteTextDropDownGridSettingData(Question _ObjQuestion
            );
        void EditGridQuestion(Question ObjQuestion, QuestionGridSetting ObjQuestionGridSetting, List<QuestionGridSettingHeader> ObjQuestionGridSettingHeader);

        void EditDropListQuestion(Question ObjQuestion, List<QuestionDropdownSetting> ObjQuestionDropdownSetting);
        void EditTextBoxQuestion(Question ObjQuestion, QuestionTextBoxSetting ObjQuestionTextBoxSetting);

        bool IsQuestionLableUniueInContract(string LableName, string DumpId);

        IEnumerable<Question> GetQuestionofDump(string DumpId);
        void UpdateQuestion(Question ObjQuestion);
        void OrderingUpQuestion(Int64 SurveyId, Int64 QuestionId);
        void OrderingDownQuestion(Int64 SurveyId, Int64 QuestionId);
        IEnumerable<Question> GetSurveyQuestionAll(Int64 SurveyId);

        IEnumerable<Question> GetSurveyNumericQuestionAll(Int64 SurveyId);

        Question GetQuestionWithIndexNo(Int64 SurveyId, int IndexNo);

        IEnumerable<Question> GetBuilderReportQuestion(Int64 ContractId, string QuaterName, string Year);
        // added by angshuman on 29-apr-2017
        IEnumerable<Question> GetBuilderReportQuestionHistory(Int64 ContractId, string QuaterName, string Year);
        IEnumerable<dynamic> GetQuestionValueWithGrid(Int64 SurveyId, string QuestionIdListFilter);
        IEnumerable<dynamic> GetQuestionByQuestionType(Int64 QuestionId, int QuestionTypeId, Int64? QuestionSettingId, bool? IsTypeOfGridQuestionHeaderSettingId);
        
    }
}
