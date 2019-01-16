using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Models
{
    public class SurveyResponseViewModel
    {
        public int ShowNCP { get; set; }
        public int ColoumnOrder { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64 ContractId { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64 QuaterId { get; set; }
        public string Quater { get; set; }
        public Int64 BuilderId { get; set; }
        public string InviteFullName { get; set; }
        public string InviteEmail { get; set; }
        public string MarketName { get; set; }
        public bool IsSurveyCompleted { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowFullname { get; set; }
        public bool ShowEmail { get; set; }
        public bool ShowCity { get; set; }
        public bool ShowState { get; set; }
        public bool ShowLot { get; set; }
        public string ExcelReportHeader { get; set; }
        public Int32 QuestionCount { get; set; }
        public List<dynamic> ProjectQuestionList { get; set; }
        public List<dynamic> Response { get; set; }
        public int rowcount { get; set; }
    }
}