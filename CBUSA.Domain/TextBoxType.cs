using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class TextBoxType
    {
        public Int32 TextBoxTypeId { get; set; }
        public string TextBoxTypeName { get; set; }
    }


    public class QuestionTextBoxSetting
    {
        public Int64 QuestionTextBoxSettingId { get; set; }
        public bool IsAlphabets { get; set; }
        public bool IsNumber { get; set; }
        public bool IsSpecialCharecter { get; set; }
        public Int32 LowerLimit { get; set; }
        public Int32 UpperLimit { get; set; }
        public Int32 TextBoxTypeId { get; set; }
        public Int64 QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public virtual TextBoxType TextBoxType { get; set; }
    }

    public class QuestionDropdownSetting
    {
        public Int64 QuestionDropdownSettingId { get; set; }
        public string Value { get; set; } //in comma separated manner
        public Int64 QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }

    public class QuestionGridSetting
    {
        public Int64 QuestionGridSettingId { get; set; }

        public Int32 Row { get; set; }
        public Int32 Column { get; set; }
        public Int64 QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<QuestionGridSettingHeader> QuestionGridSettingHeader { get; set; }
    }

    public class QuestionGridSettingHeader
    {
        public Int64 QuestionGridSettingHeaderId { get; set; }
        public string RowHeaderValue { get; set; }
        public string ColoumnHeaderValue { get; set; }
        public Int32 IndexNumber { get; set; }
        public string ControlType { get; set; }
        public string DropdownTypeOptionValue { get; set; }
        public Int64 QuestionGridSettingId { get; set; }
        public virtual QuestionGridSetting QuestionGridSetting { get; set; }
    }
}
