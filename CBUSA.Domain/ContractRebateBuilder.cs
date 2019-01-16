using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ContractRebateBuilder : BaseColumnField
    {
        public Int64 ContractRebateBuilderId { get; set; }
        public Int64 ContractId { get; set; }
        public Int64 BuilderId { get; set; }
        public decimal RebatePercentage { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Builder Builder { get; set; }
    }
}
