using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    // added start newly - angshuman on 17/09/2016 
    public class SurveyResponseNCP
    {
        public Int64 SurveyId { get; set; }
        public Int64 BuilderId { get; set; }
        public Int64 ContractId { get; set; }
        public Int64 ProjectId { get; set; }
        public Int32 Year { get; set; }
        public Int64 QuaterId { get; set; }
        public string Quater { get; set; }
        public Int64 BuilderQuaterContractProjectDetailsId { get; set; }
        public Int64 QuestionId { get; set; }
        public Int64 QuestionTypeId { get; set; }
        public int SurveyOrder { get; set; }
        public Int32 RowIndex { get; set; }
        public Int32 ColIndex { get; set; }
        public IEnumerable<BuilderQuaterContractProjectDetails> Result { get; set; }
    }
}
