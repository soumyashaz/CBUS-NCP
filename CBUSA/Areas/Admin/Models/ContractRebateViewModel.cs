using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class ContractRebateViewModel
    {
        public Int64 ContractId { get; set; }
        public Int64 ContractStatusId { get; set; }
        public decimal RebatePercentage { get; set; }
        public string ContractStatusName { get; set; }

        public Int64 BuilderId { get; set; }
    }
}