using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBUSA.Domain
{
    public class Manufacturer : BaseColumnField
    {
        public Int64 ManufacturerId { get; set; }
        [Required]
        [MaxLength(150)]
        public string ManufacturerName { get; set; }
    }
}
