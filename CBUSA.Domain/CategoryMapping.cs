using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CBUSA.Domain
{
    public class CategoryMapping : BaseColumnField
    {
        [Key]
        public Int64 TranId { get; set; }
        public Int64 BuilderId { get; set; }
        public Int64 CBUSACategoryId { get; set; }
        public Int64 BuilderCategoryId { get; set; }
        public virtual Builder Builder { get; set; }
    }
}
