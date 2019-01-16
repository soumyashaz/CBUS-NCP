using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class MarketViewModel
    {
        public Int64 MarketId { get; set; }
        public string MarketName { get; set; }
        public IEnumerable<BuildersViewModel> BuildersList { get; set; }
    }
}