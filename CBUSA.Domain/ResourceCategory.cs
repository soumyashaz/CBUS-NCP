using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ResourceCategory : BaseColumnField
    {
        public Int64 ResourceCategoryId { get; set; }
        public string ResourceCategoryName { get; set; }

        public virtual ICollection<Resource> Resource { get; set; }
    }
}
