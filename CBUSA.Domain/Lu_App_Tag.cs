using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class Lu_App_Tag : BaseColumnField
    {
        [Key]
        public Int64 TagId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DisplayValue { get; set; }

        [MaxLength(100)]
        public string SortValue { get; set; }
    }
}
