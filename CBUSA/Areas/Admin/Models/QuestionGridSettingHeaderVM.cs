using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class QuestionGridSettingHeaderViewModel
    {

        public Int64 QuestionGridSettingHeaderId { get; set; }
        public string RowHeaderValue { get; set; }
        public string ColoumnHeaderValue { get; set; }
        public Int32 IndexNumber { get; set; }
        public string ControlType { get; set; }
        //   public string DropdownTypeOptionValue { get; set; }

        public string ColoumnControlValue { get; set; }



    }

    public class GridColoumnControlViewModel
    {
        public Int32 ControIndex { get; set; }
        public string ControlType { get; set; }
        public string ControlValue { get; set; }
    }
}