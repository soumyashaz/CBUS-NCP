using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.CbusaBuilder.Models
{
    public class AddProjectStatusViewModel
    {
        public Int64 ContractId { get; set; }
        public Int64 BuilderId { get; set; }
        public Int64 QuaterId { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64 ProjectStatusId { get; set; }
        public string ContractName { get; set; }
        public string QuaterName { get; set; }
        public string Year { get; set; }
        public byte[] ContractIcon { get; set; }
        public string ManuFacturerName { get; set; }
    }
}