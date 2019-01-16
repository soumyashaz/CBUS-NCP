using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class BuildersViewModel
    {
        public Int64 BuilderId { get; set; }
        public string MarketName { get; set; }
        public string BuilderName { get; set; }
        public string BuilderEmail { get; set; }
        public string JoiningDate { get; set; }
        public bool status { get; set; }
    }

}