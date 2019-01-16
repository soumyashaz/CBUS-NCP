using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Domain
{
    public class BaseColumnField
    {
        public BaseColumnField()
        {
            RowGUID = Guid.Empty;
            CreatedOn = DateTime.Now;
            ModifiedOn = DateTime.Now;
            RowStatusId = (int)RowActiveStatus.Active;
        }
      
        public int RowStatusId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public Guid RowGUID { get; set; }
        public RowStatus RowStatus { get; set; }

    }
}
