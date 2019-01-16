using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ContentAttachment : BaseColumnField
    {
        public Int64 ContentAttachmentId { get; set; }

        public Int64 ContentId { get; set; }

        public Int64 AttachmentId { get; set; }

        [MaxLength(100)]
        public string DisplayValue { get; set; }

        public string Description { get; set; }
        
        [MaxLength(100)]
        public string VersionName { get; set; }

        public bool? VirtualAttachment { get; set; }

        [MaxLength(500)]
        public string AbsolutePath { get; set; }
    }
}
