using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ContractStatusHistory
    {
        public Int64 ContractStatusHistoryId { get; set; }
        public Int64 ContractId { get; set; }
        public Int64 ContractStatusId { get; set; }
        public DateTime? EntryDate { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual ContractStatus ContractStatus { get; set; }

    }
}
