using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class ResourceViewModel
    {
        // public Int64 ResourceId { get; set; }
        [Required (ErrorMessage="*")]
        public Int64 ResourceCategoryId { get; set; }



        [Required(ErrorMessage = "*")]
        public string Title { get; set; }

        public string Description { get; set; }
        public string Markets { get; set; }
        public string DumpId { get; set; }

        public Int64 ResourceId { get; set; }

        public Int64 ContractId { get; set; }

        public int EditMode { get; set; }


       // public string MarketTest { get; set; }

    }
}