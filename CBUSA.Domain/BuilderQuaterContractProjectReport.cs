using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class BuilderQuaterContractProjectReport : BaseColumnField
    {

        public Int64 BuilderQuaterContractProjectReportId { get; set; }
        public Int64 BuilderId { get; set; }
        public Int64 QuaterId { get; set; }
        public Int64 ContractId { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64 ProjectStatusId { get; set; }
       
        public Int64? BuilderQuaterAdminReportId { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CompleteDate { get; set; }
        public Int64? BuilderQuarterContractStatusId { get; set; }

        public virtual Builder Builder { get; set; }
        public virtual Quater Quater { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Project Project { get; set; }
        public virtual ProjectStatus ProjectStatus { get; set; }

        public virtual BuilderQuaterAdminReport BuilderQuaterAdminReport { get; set; }
        public virtual BuilderQuarterContractStatus BuilderQuarterContractStatus { get; set; }

        public virtual ICollection<BuilderQuaterContractProjectDetails> BuilderQuaterContractProjectDetails { get; set; }
    }
}
