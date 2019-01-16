using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBUSA.Areas.Admin.Models
{
    public class ReOpenBuilderNCPSurveyResponseViewModel
    {        
        public Int64 SurveyId { get; set; }
        public string SurveyName { get; set; }
        public Int64 ContractId { get; set; }
        public string ContractName { get; set; }
        public Int64 BuilderId { get; set; }
        public string BuilderName { get; set; }
        public string Year { get; set; }
        public string Quarter { get; set; }
        public bool IsEditable { get; set; }
    }
}