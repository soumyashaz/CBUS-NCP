using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ImageTag
    {
        public Int64 ImageTagId { get; set; }
        public Int64 ImageId { get; set; }
        public Int64 TagId { get; set; }
    }
}
