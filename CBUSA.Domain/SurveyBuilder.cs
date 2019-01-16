using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class SurveyBuilder : BaseColumnField
    {
        public Int64 SurveyBuilderId { get; set; }
        public DateTime SurveyStartDate { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64 BuilderId { get; set; }
        public bool IsSurveyCompleted { get; set; }
        public DateTime SurveyCompleteDate { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual Builder Builder { get; set; }

    }
}
