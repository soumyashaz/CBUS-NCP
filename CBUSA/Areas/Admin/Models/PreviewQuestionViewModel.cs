using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class PreviewQuestionViewModel
    {

        public string SurveyName { get; set; }
        public ICollection<Question> ObjQuestion { get; set; }
    }
}