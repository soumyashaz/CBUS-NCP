using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class NonResponderViewModel
    {
        public Int64 MarketId { get; set; }
        public string MarketName { get; set; }
        public Int64 BuilderId { get; set; }
        public string BuilderName { get; set; }
        public string ContractList { get; set; }
        public int CountOfParticipatingContracts { get; set; }
        public int NumberOfReportingQuarters { get; set; }
    }
}