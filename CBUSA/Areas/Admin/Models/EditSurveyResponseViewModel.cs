using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class EditSurveyResponseViewModel
    {
        public Int64 BuilderId { get; set; }
        public Int64 SurveyId { get; set; }
        public Survey ObjSurvey { get; set; }
        public List<Question> QuestionList { get; set; }
        public List<SurveyResult> ObjSurveyResult { get; set; }
    }


}