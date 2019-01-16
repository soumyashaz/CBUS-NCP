using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class QuarterContractStatus: BaseColumnField
    {
        public Int64 QuarterContractStatusId { get; set; }
        public string QuarterContractStatusName { get; set; }
    }
}
