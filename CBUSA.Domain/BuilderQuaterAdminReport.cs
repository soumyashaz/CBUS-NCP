using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class BuilderQuaterAdminReport : BaseColumnField
    {

        public Int64 BuilderQuaterAdminReportId { get; set; }
        public Int64 BuilderId { get; set; }
        public Int64 QuaterId { get; set; }
        public DateTime SubmitDate { get; set; }
        public bool IsSubmit { get; set; }

        public virtual ICollection<BuilderQuaterContractProjectReport> BuilderQuaterContractProjectReport { get; set; }
        public virtual Quater Quater { get; set; }
        public virtual Builder Builder { get; set; }
    }
}
