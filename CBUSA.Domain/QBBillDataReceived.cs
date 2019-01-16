using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBUSA.Domain
{
    public class QBBillDataReceived : BaseColumnField
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 TranId { get; set; }
        public string QBTxnID { get; set; }
        public DateTime? TimeCreated { get; set; }
        public DateTime? TimeModified { get; set; }
        public string EditSequence { get; set; }
        public Int64 TxnNumber { get; set; }
        public string VendorRefListID { get; set; }
        public string VendorRefFullName { get; set; }
        public string APAcountRefListId { get; set; }
        public string APAcountRefFullName { get; set; }
        public DateTime? TxnDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Double AmountDue { get; set; }
        public string TermsRefListId { get; set; }
        public string TermsRefFullName { get; set; }
        public string Memo { get; set; }
        public bool IsPaid { get; set; }
        public double OpenAmount { get; set; }
        public Int64 BuilderId { get; set; }
        public virtual Builder Builder { get; set; }
    }
}
