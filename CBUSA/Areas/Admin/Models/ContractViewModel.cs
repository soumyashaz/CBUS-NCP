using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class ContractViewModel
    {

        public Int64 ContractId { get; set; }
        [Required]
        [MaxLength(150)]
        public string ContractName { get; set; }
        public byte[] ContractIcon { get; set; }
        [Required]
        [MaxLength(150)]
        public string Label { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EntryDeadline { get; set; }
        public DateTime? ContractFrom { get; set; }
        public DateTime? ContractTo { get; set; }
        public string ContractDeliverables { get; set; }
        public string PrimaryManufacturer { get; set; }
        public Int64? ManufacturerId { get; set; }
        [Required]
        public string Website { get; set; }
        [Required]
        public string Products { get; set; }
        public Int32 ContractStatusId { get; set; }

        public string DumpId { get; set; }
        public int Rowstatus { get; set; }
        public bool IsReportable { get; set; }

    }
}