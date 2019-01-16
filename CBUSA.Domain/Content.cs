using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class Content : BaseColumnField
    {
        public Int64 ContentId { get; set; }

        public Int64 SectionId { get; set; }

        public Int64 ContractId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DisplayValue { get; set; }

        [MaxLength(100)]
        public string SortValue { get; set; }

        public string ContentText { get; set; }

        public virtual ICollection<ContentMarket> ContentMarket { get; set; }

        public virtual ICollection<ContentImage> ContentImage { get; set; }

        public virtual ICollection<ContentAttachment> ContentAttachment { get; set; }
    }
}
