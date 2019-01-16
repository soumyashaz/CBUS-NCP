using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class Resource : BaseColumnField
    {

        public Int64 ResourceId { get; set; }

        [MaxLength(250)]
        public string FileLocation { get; set; }

        [MaxLength(250)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }
        public Int64 ResourceCategoryId { get; set; }
        public Int64? ContractId { get; set; }
        public string DumpId { get; set; }
        public virtual ResourceCategory ResourceCategory { get; set; }
        public virtual Contract Contract { get; set; }

        public virtual ICollection<ResourceMarket> ResourceMarket { get; set; }

    }
}
