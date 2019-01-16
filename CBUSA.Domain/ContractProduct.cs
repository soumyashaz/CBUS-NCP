using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ContractProduct
    {
        public Int64 ContractProductId { get; set; }
        public Int64 ContractId { get; set; }
        public Int64 ProductId { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Product Product { get; set; }
    }
}
