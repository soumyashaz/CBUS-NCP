using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Models
{
    public class ProductCategoryView
    {

        public int CatId { get; set; }
        public string CatName { get; set; }
        public Nullable<int> ParentId { get; set; }

        public Int32 AEProductId { get; set; }
        public string ProductName { get; set; }
        public Int32 ProductCategoryId { get; set; }

        public IEnumerable<ProductCategoryView> ChildCategoryList { get; set; }
    }
}

