using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBUSA.Areas.Admin.Models
{
    public class ContractStatusViewModel
    {
        public Int64 ContractStatusId { get; set; }
        [Required(ErrorMessage = "*")]
        public string ContractStatusName { get; set; }
        public bool IsNonEditable { get; set; }
        public bool IsNonDeletable { get; set; }


    }
}