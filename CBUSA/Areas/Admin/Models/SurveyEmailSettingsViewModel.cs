using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBUSA.Areas.Admin.Models
{

    public class SurveySettingsViewModel
    {

        public Int64 SurveyId { get; set; }
        [Required]
        public string SenderEmail { get; set; }
        public bool RemainderForTakeSurvey { get; set; }
        public int DayBeforeSurveyEnd { get; set; }

        public bool RemainderForTakeSurveySecond { get; set; }
        public Int32 DayBeforeSurveyEndSecond { get; set; }


        public bool RemainderForTakeSurveyThird { get; set; }
        public Int32 DayBeforeSurveyEndThird { get; set; }

        public bool RemainderForContinueSurvey { get; set; }
        public int DayAfterSurveyEnd { get; set; }
        public string InviteEmailDumpId { get; set; }
        public string RemainderEmailDumpId { get; set; }
        public string ContinueEmailDumpId { get; set; }
    }


    public class SurveyEmailSettingsViewModel
    {

        public Int64 SurveyId { get; set; }
        public string EmailSubject { get; set; }
        [AllowHtml]
        public string EmailBody { get; set; }

        public string InviteEmailSubject { get; set; }
        [AllowHtml]
        public string InviteEmailBody { get; set; }

        public string RemainderEmailSubject { get; set; }
        [AllowHtml]
        public string RemainderEmailBody { get; set; }

        public string SaveContinueEmailSubject { get; set; }
        [AllowHtml]
        public string SaveContinueEmailBody { get; set; }

        public string DumpId { get; set; }








    }
}