using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBUSA.Domain;
namespace CBUSA.Areas.Admin.Models
{
    public class ContractProductViewModel
    {
        public Int64 ProductCategoryId { get; set; }
      
        public string ProductCategoryName { get; set; }
        public List<Product> Products { get; set; }
   
    }
}