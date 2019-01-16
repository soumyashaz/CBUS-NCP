using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class ResourceMarket
    {

        public Int64 ResourceMarketId { get; set; }
        public Int64 MarketId { get; set; }
        public Int64 ResourceId { get; set; }
        public virtual Resource Resource { get; set; }
        public virtual Market Market { get; set; }
    }
}
