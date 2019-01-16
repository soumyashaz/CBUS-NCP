using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CBUSA.Domain
{
    public class ContractCompliance : BaseColumnField
    {
        public Int64 ContractComplianceId { get; set; }
        public bool EstimatedValue { get; set; }
        public bool ActualValue { get; set; }
        public bool IsDirectQuestion { get; set; }
        public Int64? QuestionId { get; set; }

        [Column(TypeName = "xml")]
        public string ComplianceFormula { get; set; }

        XmlDocument _ComplianceDocument;
        [NotMapped]
        public XmlDocument ComplianceDocument
        {
            get
            {
                if (_ComplianceDocument == null)
                {
                    _ComplianceDocument = new XmlDocument();
                    _ComplianceDocument.LoadXml(ComplianceFormula);
                }
                return _ComplianceDocument;
            }
        }
        public Int64 ContractId { get; set; }
        public Int64 SurveyId { get; set; }
        public Int64? QuaterId { get; set; }
        public Quater Quater { get; set; }
        public Contract Contract { get; set; }
        public Survey Survey { get; set; }
        public Question Question { get; set; }

    }

    public class ContractComplianceBuilder
    {
        public Int64 ContractComplianceBuilderId { get; set; }
        public decimal EstimatedValue { get; set; }
        public decimal ActualValue { get; set; }
        public Int64 BuilderId { get; set; }
        public Int64 ContractId { get; set; }
        public Contract Contract { get; set; }
        public Builder Builder { get; set; }

    }


}
