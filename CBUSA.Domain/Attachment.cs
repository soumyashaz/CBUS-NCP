using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class Attachment : BaseColumnField
    {
        public Int64 AttachmentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DisplayValue { get; set; }

        public string Description { get; set; }

        [Required]
        [MaxLength(10)]
        public string Format { get; set; }

        [MaxLength(500)]
        public string RelativePath { get; set; }

        [MaxLength(100)]
        public string FileName { get; set; }

        public Int64? SizeInBytes { get; set; }

        [MaxLength(100)]
        public string VersionName { get; set; }

        public bool? VirtualAttachment { get; set; }

        [MaxLength(500)]
        public string AbsolutePath { get; set; }
    }
}
