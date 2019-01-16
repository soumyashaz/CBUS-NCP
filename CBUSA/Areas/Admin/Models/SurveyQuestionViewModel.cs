using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class SurveyQuestionViewModel
    {

        public Int64 QuestionId { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public Int64 SurveyId { get; set; }
        public bool IsSurveyPublished { get; set; }
        public Int32 SurveyOrder { get; set; }
        public bool OrderUp { get; set; }
        public bool OrderDown { get; set; }
    }
}