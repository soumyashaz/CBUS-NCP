using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBUSA.Areas.Admin.Models
{
    public class SurveyViewModel
    {
        public Int64 SurveyId { get; set; }

        [Required(ErrorMessage = "*")]
        public string SurveyName { get; set; }
        [Required(ErrorMessage = "*")]
        public string SurveyLabel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Int64 ContractId { get; set; }
        public string PublishDate { get; set; }
        public string Archive { get; set; }
        public string SurveyStatus { get; set; }
        public string ContractName { get; set; }
        public string LastDate { get; set; }

        public string Response { get; set; }
        public string ResponseCom { get; set; }
        public string ResponseInCom { get; set; }
        public string ResponsePend { get; set; }
        public string MarketList { get; set; }

        public bool IsEnrolment { get; set; }
        public bool IsPublished { get; set; }

        public int IsEnrollmentChange { get; set; }
        public int EnrollmentSurveyId { get; set; }
        public string Year { get; set; }
        public string Quater { get; set; }

        public bool IsNcpSurvey { get; set; }

        public List<SelectListItem> YearList { get; set; }
        public List<SelectListItem> QuaterList { get; set; }
        public int rowcount { get; set; }

    }
}