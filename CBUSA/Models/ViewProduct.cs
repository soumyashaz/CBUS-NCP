using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CBUSA.Models
{
    public class ViewProduct
    {
        public IList<SelectListItem> CategorieList { get; set; }
        public IList<SelectListItem> ProductList { get; set; }
    }
}