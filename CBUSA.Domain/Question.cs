using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class Question : BaseColumnField
    {
        public Int64 QuestionId { get; set; }
        [Required]
        public string QuestionValue { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsFileNeedtoUpload { get; set; }
        public Int32 SurveyOrder { get; set; }
        public Int32 QuestionTypeId { get; set; }
        public Int64 SurveyId { get; set; }
        public virtual QuestionType QuestionType { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual ICollection<QuestionTextBoxSetting> QuestionTextBoxSetting { get; set; }
        public virtual ICollection<QuestionGridSetting> QuestionGridSetting { get; set; }
        public virtual ICollection<QuestionDropdownSetting> QuestionDropdownSetting { get; set; }
    }
}
