using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class AttachmentTag
    {
        public Int64 AttachmentTagId { get; set; }
        public Int64 AttachmentId { get; set; }
        public Int64 TagId { get; set; }
    }
}
