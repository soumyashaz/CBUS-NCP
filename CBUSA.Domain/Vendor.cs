using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class Vendor : BaseColumnField
    {
        public Int64 VendorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; } // OR Company Name

        
    }
}
