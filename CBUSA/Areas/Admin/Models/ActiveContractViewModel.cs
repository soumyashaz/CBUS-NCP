using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class ActiveContractViewModel
    {
        public int rowcount { get; set; }
        public Int64 ConractId { get; set; }
        public string ConractName { get; set; }
        public string Icon { get; set; }
        public byte[] ContractIcon { get; set; }
        public string ManuFacturerName { get; set; }
        public Int64 ManufaturerId { get; set; }
        public string ProductList { get; set; }
        public string checksubmit { get; set; }
        public string Estimated { get; set; }
        public string Percentage { get; set; }
       // public string Website { get; set; }

        public int BuilderCount { get; set; }
        public string ContractStatus { get; set; }
        public string ReportStatus { get; set; }
        public string ReportingLabel { get; set; }
        public string ReportingLabelColor { get; set; }
        public string CheckStatus { get; set; }
        public string ContractFrom { get; set; }
        public string ContractTo { get; set; }
        public bool IsAllRowVisible { get; set; }
        public string Website { get; set; }
        public List<string> Websiteslist { get; set; }
        public List<string> Manufactererlist { get; set; }
        public string ContractTerm { get; set; }

        //public List<string> Websiteslist { get; set; }
        //public List<string> Manufactererlist { get; set; }

    }
}