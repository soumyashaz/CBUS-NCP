using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBUSA.Areas.Admin.Models
{
    public class NcpComplianceViewModel
    {
        public Int64 ContractId { get; set; }
        public Int64? EstimatedSurveyId { get; set; }
        public Int64? EstimatedQuestionId { get; set; }
        public string EstimatedComposeFormula { get; set; }
        public bool IsEstimateDirectQuestion { get; set; }
        public Int64? ActualsSurveyId { get; set; }
        public Int64? ActualQuestionId { get; set; }
        public string ActualComposeFormula { get; set; }
        public bool IsActualDirectQuestion { get; set; }
        public bool IsEstimated { get; set; }

        public Int64 Year { get; set; }
        public string Quater { get; set; }

        public List<SelectListItem> YearList { get; set; }
        public List<SelectListItem> QuaterList { get; set; }

    }



}