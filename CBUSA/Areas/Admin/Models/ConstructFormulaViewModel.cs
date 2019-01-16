using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBUSA.Areas.Admin.Models
{
    public class ConstructFormulaViewModel
    {
        public Int64 ConstructFormulaId { get; set; }
        public Int64 SurveyId { get; set; }
        [Required(ErrorMessage = "*")]
        public string SurveyName { get; set; }
        [Required(ErrorMessage = "*")]
        public Int64 ContractId { get; set; }
        public string ContractName { get; set; }        
        public string MarketList { get; set; }
        public Int64 MarketId { get; set; }
        public string MarketName { get; set; }
        public Int64 QuestionId { get; set; }
        public string Year { get; set; }
        public string Quarter { get; set; }
        public bool IsNcpSurvey { get; set; }
        public string Question { get; set; }
        public string QuestionColumn { get; set; }
        public string QuestionColumnId { get; set; }
        public string QuestionColumnText { get; set; }
        public int ColumnIndexNumber { get; set; }
        public string QuestionColumnValueText { get; set; }
        public string QuestionColumnValueId { get; set; }
        public string QuestionColumnValue { get; set; }
        public string QuestionColumnHeaverValue { get; set; }
        public int RowIndexNumber { get; set; }
        public string Formula { get; set; }
        public int QuestionTypeId { get; set; }
        public List<SelectListItem> YearList { get; set; }
        public List<SelectListItem> QuarterList { get; set; }
        public List<SelectListItem> QuestionList { get; set; }
        public List<SelectListItem> QuestionColumnList { get; set; }
        public List<SelectListItem> QuestionColumnValueList { get; set; }
        public List<SelectListItem> FormulaList { get; set; }
        public List<SelectListItem> MarketListData { get; set; }
        public string FormulaBuild { get; set; }
    }
}