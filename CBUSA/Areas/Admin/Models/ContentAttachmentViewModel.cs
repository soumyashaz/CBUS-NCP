using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class ContentAttachmentViewModel
    {
        public Int64 ContentAttachmentId { get; set; }

        public Int64 ContentId { get; set; }

        public Int64 AttachmentId { get; set; }

        public string FileName { get; set; }

        public string FileSize { get; set; }

        public string DisplayValue { get; set; }

        public string Description { get; set; }

        public string VersionName { get; set; }

        public bool? VirtualAttachment { get; set; }

        public string AbsolutePath { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}