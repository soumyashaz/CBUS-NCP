using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ContractStatus : BaseColumnField
    {
        public Int64 ContractStatusId { get; set; }
        public string ContractStatusName { get; set; }
        public bool IsNonEditable { get; set; }
        public int Order { get; set; }
        public virtual ICollection<ContractRebate> ContractRebate { get; set; }
    }
}
