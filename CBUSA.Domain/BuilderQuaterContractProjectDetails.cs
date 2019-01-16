using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class BuilderQuaterContractProjectDetails : BaseColumnField
    {
        public Int64 BuilderQuaterContractProjectDetailsId { get; set; }
        public string Answer { get; set; }
        public Int32 RowNumber { get; set; }
        public Int32 ColumnNumber { get; set; }
        public Int64 QuestionId { get; set; }
        public Int64 BuilderQuaterContractProjectReportId { get; set; }
        public string FileName { get; set; }
        public virtual Question Question { get; set; }
        public virtual BuilderQuaterContractProjectReport BuilderQuaterContractProjectReport { get; set; }

    }
}
