using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Models
{
    public class CategoryTreeViewModel
    {
        public Int64 CategoryId { get; set; }
        public string CategoryName { get; set; }
        IEnumerable<SubCategoryTreeViewModel> SubCategory { get; set; }
        public bool HasSubCategory { get; set; }
    }

    public class SubCategoryTreeViewModel
    {
        public Int64 CategoryId { get; set; }
        public string CategoryName { get; set; }
    }



    public class ProductFetchViewModel
    {
        public string Name { get; set; }
        //  public List<test> SubCategories { get; set; }
    }
}