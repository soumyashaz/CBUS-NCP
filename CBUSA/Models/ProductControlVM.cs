using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Models
{
    public class ProductControlVM
    {
        public List<ProductCategory> ObjProductCategory { get; set; }
        public int Flag { get; set; }

    }
}