using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBUSA.Domain
{
    public class SubmitReport : BaseColumnField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 SubmitReportTranId { get; set; }
        public DateTime SubmitReportTranDate { get; set; }
        public Int64 SubmitReportQuarterId { get; set; }
        public Int64 BuilderId { get; set; }
        public DateTime PurchaseFromDate { get; set; }
        public DateTime PurchaseToDate { get; set; }
        public string VendorId { get; set; }
        public string CategoryId { get; set; }
        public double BillAmountEntered { get; set; }
        public double BillAmountPaid { get; set; }
        public double SalesTaxAmount { get; set; }
        public double BillAmountReported { get; set; }
        public string Comments { get; set; }
        public string VendorType { get; set; }
        public virtual Builder Builder { get; set; }
    }
}
