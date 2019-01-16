using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class SurveyResponseEditStatus : BaseColumnField
    {
        public Int64 SurveyResponseEditStatusId { get; set; }
        public Int64 BuilderId { get; set; }
        public Int64 QuaterId { get; set; }
        public Int64 ContractId { get; set; }
        public bool IsEditable { get; set; }
        public virtual Builder Builder { get; set; }
        public virtual Quater Quater { get; set; }
        public virtual Contract Contract { get; set; }
    }
}
