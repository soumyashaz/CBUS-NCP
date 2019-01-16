using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ContractBuilder :BaseColumnField
    {

        public Int64 ContractBuilderId { get; set; }
        public Int64 ContractId { get; set; }
        public Int64 BuilderId { get; set; }
        public DateTime JoiningDate { get; set; }
        public Int64 ContractStatusId { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Builder Builder { get; set; }
        public virtual ContractStatus ContractStatus { get; set; }


    }
}
