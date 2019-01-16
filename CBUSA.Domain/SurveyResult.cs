using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class SurveyResult : BaseColumnField
    {
        public Int64 SurveyResultId { get; set; }
        public string Answer { get; set; }
        public Int32 RowNumber { get; set; }
        public Int32 ColumnNumber { get; set; }
        public Int64 QuestionId { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64 BuilderId { get; set; }
        public string FileName { get; set; }
        public virtual Question Question { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual Builder Builder { get; set; }
    }
}
