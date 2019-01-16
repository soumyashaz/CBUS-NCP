using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
   public class RowStatus
    {
        [Key]
        public int RowStatusId { get; set; }
       
        public string RowStatusDescription { get; set; }
        public Guid RowGUID { get; set; }
    }
}
