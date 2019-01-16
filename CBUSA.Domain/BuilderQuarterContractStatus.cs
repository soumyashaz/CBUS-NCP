using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class BuilderQuarterContractStatus: BaseColumnField
    {
        public Int64 BuilderQuarterContractStatusId { get; set; }
        public Int64 BuilderId { get; set; }
        public Int64 QuaterId { get; set; }
        public Int64 ContractId { get; set; }
        public DateTime SubmitDate { get; set; }
        public Int64 QuarterContractStatusId { get; set; }
        public Int64 ProjectReportStatusId { get; set; }
        public Int64 BuilderQuaterAdminReportId { get; set; }
        public virtual BuilderQuaterAdminReport BuilderQuaterAdminReport { get; set; }
        public virtual Quater Quater { get; set; }
        public virtual Builder Builder { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual QuarterContractStatus QuarterContractStatus { get; set; }
        public virtual ProjectReportStatus ProjectReportStatus { get; set; }
    }
}
