using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class ContractResourceViewModel
    {

        public Int64 ResourceId { get; set; }
        public string ResourceLocation { get; set; }
      public  string ResourceName { get; set; }
      public  string ResourceTitle { get; set; }
      public string upload { get; set; }
      public  string ResourceMarketList { get; set; }
      public string css { get; set; }
      public bool IsAllRowVisible { get; set; }

    }
}