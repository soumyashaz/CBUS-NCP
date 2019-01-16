using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBUSA.Domain;
namespace CBUSA.Areas.TakeSurvey.Models
{
    public class TakeSurveyViewModel
    {
        public Survey Survey { get; set; }
        public Int64 BuilderId { get; set; }
        public IEnumerable<SurveyResult> SurveyResult { get; set; }
    }
}