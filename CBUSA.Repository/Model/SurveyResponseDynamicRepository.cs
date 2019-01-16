using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository.Model
{
    public class SurveyResponseDynamicRepository
    {
        public Int64 ProjectId { get; set; }
        public Int64 QuestionId { get; set; }
        public string QuestionValue { get; set; }
        public Int32 QuestionTypeId { get; set; }
        public Int32 SurveyOrder { get; set; }
        public Int64 SurveyId { get; set; }
        public Int32 RowIndex { get; set; }
        public Int32 ColIndex { get; set; }
        public Int64 QuestionGridSettingId { get; set; }        
    }
}
