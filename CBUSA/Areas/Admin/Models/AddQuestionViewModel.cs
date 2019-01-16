using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBUSA.Domain;
namespace CBUSA.Areas.Admin.Models
{
    public class AddQuestionViewModel
    {
        public Question ObjQuestion { get; set; }
        public QuestionTextBoxSetting ObjTextBoxSetting { get; set; }
        public QuestionDropdownSetting ObjDropDownSetting { get; set; }
        public QuestionGridSetting ObjGridSetting { get; set; }
        public TextBoxType ObjTextBoxType { get; set; }

        public QuestionGridSettingHeaderViewModel ObjQuestionGridSettingHeader { get; set; }
    }
}