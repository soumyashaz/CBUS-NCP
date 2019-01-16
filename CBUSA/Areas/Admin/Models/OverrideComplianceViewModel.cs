using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBUSA.Areas.Admin.Models
{

    public class OverrideComplianceViewModel
    {
        public List<SelectListItem> BuilderList { get; set; }
        public List<BuilderComplianceFactorViewModel> ContractBuilderComplianceList { get; set; }

        public int Flag { get; set; }
        public int Count { get; set; }
        public Int64 ContractId { get; set; }
    }

    public class BuilderComplianceFactorViewModel
    {
        //public 

        public decimal OrginalEstilamteValue { get; set; }
        public decimal OrginalActualValue { get; set; }
        public decimal NewlEstilamteValue { get; set; }
        public decimal NewActualValue { get; set; }
        public Int64 BuilderId { get; set; }

    }
}