using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBUSA.Domain;
namespace CBUSA.Areas.CbusaBuilder.Models
{
    public class BuilderReportViewModel
    {
        public Int64 ContractId { get; set; }
        public string ContractName { get; set; }
        public byte[] ContractIcon { get; set; }
        public Int64 BuilderId { get; set; }
        public string QuaterName { get; set; }
        public Int64 QuaterId { get; set; }
        public Int64 SurveyId { get; set; }
        public List<Question> QuestionList { get; set; }
        public List<Project> ProjectList { get; set; }

        public List<ProjectViewModel> ProjectListVM { get; set; }
        
        public List<BuilderReportSubmitViewModel> SubmitReport { get; set; }

    }

    public class BuilderReportSubmitViewModel
    {
        public Int64 ProjectId { get; set; }
        public string Answer { get; set; }
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public Int32 QuestionTypeId { get; set; }
        public Int64 QuestionId { get; set; }
        public string FileName { get; set; }
    }




}