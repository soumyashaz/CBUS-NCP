using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CBUSA.Models
{
    public class ViewStatus
    {
        [Key]
        public Int16 StatusId { get; set; }

        [Required]
        [MaxLength(50)]
        public string StatusName { get; set; }

    }
}