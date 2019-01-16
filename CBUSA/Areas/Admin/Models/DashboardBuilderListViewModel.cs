using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class DashboardBuilderListViewModel
    {
        public Int64 BuilderId { get; set; }
        public string BuilderName { get; set; }
        public Int64 MarketId { get; set; }
        public string MarketName { get; set; }
        public string ContractStatus { get; set; }
        public string LastActivityDate { get; set; }
    }
}