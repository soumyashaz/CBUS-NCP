using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class SurveyResponse
    {

        public Int64 QuestionId { get; set; }
        public Int64 QuestionTypeId { get; set; }
        public int SurveyOrder { get; set; }
        public Int32 RowIndex { get; set; }
        public Int32 ColIndex { get; set; }
        public IEnumerable<SurveyResult> Result { get; set; }


    }
}
