using CBUSA.Areas.CbusaBuilder.Models;
using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class EditProjectResponseViewModel
    {
        public Int64 BuilderId { get; set; }
        public Int64 QuaterId { get; set; }
        public Int64 ContractId { get; set; }
        public Int64 SurveyId { get; set; }
        public Survey ObjSurvey { get; set; }
        public Int64 ProjectId { get; set; }
        public List<Question> QuestionList { get; set; }
        public List<BuilderReportSubmitViewModel> ObjSubmitReportResult { get; set; }
    }


    
}