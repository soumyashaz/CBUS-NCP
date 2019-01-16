using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBUSA.Domain
{
    public class QBCategoryDataReceived : BaseColumnField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 TranId { get; set; }
        [Required]
        [MaxLength(1000)]
        public string ListID { get; set; }
        public DateTime? TimeCreated { get; set; }
        public DateTime? TimeModified { get; set; }
        public string EditSequence { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public Int32 Sublevel { get; set; }
        public string AccountType { get; set; }
        [Required]
        [MaxLength(100)]
        public string AccountNumber { get; set; }
        public string Desc { get; set; }
        public double Balance { get; set; }
        public double TotalBalance { get; set; }
        public Int32 TaxLineID { get; set; }
        public string TaxLineName { get; set; }
        public string CashFlowClassification { get; set; }
        public Int64 BuilderId { get; set; }
        public virtual Builder Builder { get; set; }
    }
}
