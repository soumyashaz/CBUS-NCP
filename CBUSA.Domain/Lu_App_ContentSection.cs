using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class Lu_App_ContentSection : BaseColumnField
    {
        [Key]
        public Int64 SectionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DisplayValue { get; set; } 

        [MaxLength(100)]
        public string SortValue { get; set; }

        public byte AboveBar { get; set; }

        [MaxLength(100)]
        public string ToolTip { get; set; }

        [MaxLength(50)]
        public string Icon { get; set; }

        public bool InternalOnly { get; set; }

        public bool JoinedOnly { get; set; }

        public bool AutoAdd { get; set; }

    }
}
