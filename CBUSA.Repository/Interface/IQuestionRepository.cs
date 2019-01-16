using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository.Interface
{
    public interface IQuestionTypeRepository : IRepository<QuestionType>
    {
        
    }

    public interface IQuestionRepository : IRepository<Question>
    {
        IEnumerable<Question> GetNumericQuestion(Int64 SurveyId);
        Question GetQuestionWithIndexNo(Int64 SurveyId, int IndexNo);
        IEnumerable<dynamic> GetQuestionValueWithGrid(Int64 SurveyId, string QuestionIdListFilter);
        IEnumerable<dynamic> GetGridQuestionHeader(Int64 QuestionId, Int64? QuestionSettingId, bool? IsTypeOfGridQuestionHeaderSettingId);
    }

    public interface ITextBoxTypeRepository : IRepository<TextBoxType>
    {
    }

    public interface IQuestionTextBoxSettingRepository : IRepository<QuestionTextBoxSetting>
    {
    }

    public interface IQuestionDropdownSettingRepository : IRepository<QuestionDropdownSetting>
    {
    }

    public interface IQuestionGridSettingRepository : IRepository<QuestionGridSetting>
    {
    }

    public interface IQuestionGridSettingHeaderRepository : IRepository<QuestionGridSettingHeader>
    {
    }

    
}
