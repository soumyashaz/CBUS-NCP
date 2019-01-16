using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBUSA.Domain;
namespace CBUSA.Areas.Admin.Models
{
    public class MarketContractBuilderViewModel
    {
        public Int64 MarketId { get; set; }
        public string MarketName { get; set; }
        public List<Builder> Builders { get; set; }
    
     }
}