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
}
