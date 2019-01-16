using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ContractRebate : BaseColumnField
    {
        public Int64 ContractRebateId { get; set; }
        public Int64 ContractId { get; set; }
        public Int64 ContractStatusId { get; set; }
        public decimal RebatePercentage { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual ContractStatus ContractStatus { get; set; }

    }
}
