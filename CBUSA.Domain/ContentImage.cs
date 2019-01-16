using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ContentImage
    {
        public Int64 ContentImageId { get; set; }
        public Int64 ContentId { get; set; }
        public Int64 ImageId { get; set; }
    }
}
