using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class SurveyBuilderEmailSendDetails : BaseColumnField
    {
        public Int64 SurveyBuilderEmailSendDetailsId { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64 BuilderId { get; set; }

        public DateTime SendDate { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual Builder Builder { get; set; }
    }
}
