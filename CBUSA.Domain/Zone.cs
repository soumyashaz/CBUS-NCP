using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
   public class Zone : BaseColumnField
    {
       public int ZoneId { get; set; }
       [Required]
       [MaxLength(250)]
       public string ZoneName { get; set; }
       public virtual ICollection<Market> Market { get; set; } 
    }
}
