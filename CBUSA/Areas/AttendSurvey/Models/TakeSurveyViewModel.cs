using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
namespace CBUSA.Areas.AttendSurvey.Models
{
    public class TakeSurveyViewModel
    {


        public Int64 BuilderId { get; set; }
        public Int64 SurveyId { get; set; }
        public Survey ObjSurvey { get; set; }
        public bool IsSurveyComplete { get; set; }

        public byte[] ContractImage { get; set; }

        public string ManufacturerName { get; set; }
        //public List<Question> QuestionList { get; set; }
        //public List<SurveyResult> ObjSurveyResult { get; set; }
        public TakeSurveyQuestionViewModel ObjTakeSurveyQuestion { get; set; }
    }

    public class TakeSurveyQuestionViewModel
    {
        public Question Question { get; set; }
        public int CurrentQuestionIndex { get; set; }
        public int TotalQuestion { get; set; }
        public List<SurveyResult> ObjSurveyResult { get; set; }

    }

}
