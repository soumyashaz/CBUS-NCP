using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class ResourceCategoryViewModel
    {
        public Int64 ResourceCategoryId { get; set; }

        [Required(ErrorMessage = "*")]
        public string ResourceCategoryName { get; set; }
        public Int64 ContractId { get; set; }
        public Int64 CategoryId { get; set; }
        public string css { get; set; }
        public bool IsActive { get; set; }

    }
}